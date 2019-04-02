/**
* @copyright Copyright (c) 2017 B-com http://www.b-com.com/
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

//#define VIDEO_INPUT
#define NDEBUG

using SolAR.Api.Display;
using SolAR.Api.Features;
using SolAR.Api.Geom;
using SolAR.Api.Image;
using SolAR.Api.Input.Files;
using SolAR.Api.Solver.Pose;
using SolAR.Core;
using SolAR.Datastructure;
using UniRx;
using XPCF.Api;

namespace SolAR.Samples
{
    public class FiducialPipeline : AbstractPipeline
    {
        public FiducialPipeline(IComponentManager xpcfComponentManager) : base(xpcfComponentManager)
        {
            binaryMarker = Create<IMarker2DSquaredBinary>("SolARMarker2DSquaredBinaryOpencv");

#if !NDEBUG
            imageViewer = Create<IImageViewer>("SolARImageViewerOpencv");
            imageViewerGrey = Create<IImageViewer>("SolARImageViewerOpencv", "grey");
            imageViewerBinary = Create<IImageViewer>("SolARImageViewerOpencv", "binary");
            imageViewerContours = Create<IImageViewer>("SolARImageViewerOpencv", "contours");
            imageViewerFilteredContours = Create<IImageViewer>("SolARImageViewerOpencv", "filteredContours");
#endif

            imageFilterBinary = Create<IImageFilter>("SolARImageFilterBinaryOpencv");
            imageConvertor = Create<IImageConvertor>("SolARImageConvertorOpencv");
            contoursExtractor = Create<IContoursExtractor>("SolARContoursExtractorOpencv");
            contoursFilter = Create<IContoursFilter>("SolARContoursFilterBinaryMarkerOpencv");
            perspectiveController = Create<IPerspectiveController>("SolARPerspectiveControllerOpencv");
            patternDescriptorExtractor = Create<IDescriptorsExtractorSBPattern>("SolARDescriptorsExtractorSBPatternOpencv");

            patternMatcher = Create<IDescriptorMatcher>("SolARDescriptorMatcherRadiusOpencv");
            patternReIndexer = Create<ISBPatternReIndexer>("SolARSBPatternReIndexer");

            img2worldMapper = Create<IImage2WorldMapper>("SolARImage2WorldMapper4Marker2D");
            PnP = Create<I3DTransformFinderFrom2D3D>("SolARPoseEstimationPnpOpencv");
#if !NDEBUG
            overlay2DContours = Create<I2DOverlay>("SolAR2DOverlayOpencv", "contours");
            overlay2DCircles = Create<I2DOverlay>("SolAR2DOverlayOpencv", "circles");
#endif

            greyImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            binaryImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);

            contours = new Contour2DfList().AddTo(subscriptions);
            filtered_contours = new Contour2DfList().AddTo(subscriptions);
            patches = new ImageList().AddTo(subscriptions);
            recognizedContours = new Contour2DfList().AddTo(subscriptions);
            recognizedPatternsDescriptors = new DescriptorBuffer().AddTo(subscriptions);
            markerPatternDescriptor = new DescriptorBuffer().AddTo(subscriptions);
            patternMatches = new DescriptorMatchVector().AddTo(subscriptions);
            pattern2DPoints = new Point2DfList().AddTo(subscriptions);
            img2DPoints = new Point2DfList().AddTo(subscriptions);
            pattern3DPoints = new Point3DfList().AddTo(subscriptions);
            //CamCalibration K;

            // components initialisation
            binaryMarker.loadMarker().Check();
            patternDescriptorExtractor.extract(binaryMarker.getPattern(), markerPatternDescriptor).Check();
            var binaryMarkerSize = binaryMarker.getSize();

            var patternSize = binaryMarker.getPattern().getSize();

            patternDescriptorExtractor.BindTo<IConfigurable>().getProperty("patternSize").setIntegerValue(patternSize);
            patternReIndexer.BindTo<IConfigurable>().getProperty("sbPatternSize").setIntegerValue(patternSize);

            // NOT WORKING ! initialize image mapper with the reference image size and marker size
            var img2worldMapperConf = img2worldMapper.BindTo<IConfigurable>();
            img2worldMapperConf.getProperty("digitalWidth").setIntegerValue(patternSize);
            img2worldMapperConf.getProperty("digitalHeight").setIntegerValue(patternSize);
            img2worldMapperConf.getProperty("worldWidth").setFloatingValue(binaryMarkerSize.width);
            img2worldMapperConf.getProperty("worldHeight").setFloatingValue(binaryMarkerSize.height);
        }

        public override Sizef GetMarkerSize() { return binaryMarker.getSize(); }
        public override void SetCameraParameters(Matrix3x3f intrinsics, Vector5f distorsion)
        {
            PnP.setCameraParameters(intrinsics, distorsion);
        }

        public override FrameworkReturnCode Proceed(Image inputImage, Transform3Df pose)
        {
            // Convert Image from RGB to grey
            imageConvertor.convert(inputImage, greyImage, Image.ImageLayout.LAYOUT_GREY).Check();

            // Convert Image from grey to black and white
            imageFilterBinary.filter(greyImage, binaryImage).Check();

            // Extract contours from binary image
            contoursExtractor.extract(binaryImage, contours).Check();
#if !NDEBUG
            var contoursImage = binaryImage.copy();
            overlay2DContours.drawContours(contours, contoursImage);
#endif
            // Filter 4 edges contours to find those candidate for marker contours
            contoursFilter.filter(contours, filtered_contours).Check();

#if !NDEBUG
            var filteredContoursImage = binaryImage.copy();
            overlay2DContours.drawContours(filtered_contours, filteredContoursImage);
#endif
            // Create one warpped and cropped image by contour
            perspectiveController.correct(binaryImage, filtered_contours, patches).Check();

            // test if this last image is really a squared binary marker, and if it is the case, extract its descriptor
            if (patternDescriptorExtractor.extract(patches, filtered_contours, recognizedPatternsDescriptors, recognizedContours) == FrameworkReturnCode._SUCCESS)
            {

#if !NDEBUG
                var std__cout = new System.Text.StringBuilder();
                /*
                LOG_DEBUG("Looking for the following descriptor:");
                for (var i = 0; i < markerPatternDescriptor.getNbDescriptors() * markerPatternDescriptor.getDescriptorByteSize(); i++)
                {

                    if (i % patternSize == 0)
                        std__cout.Append("[");
                    if (i % patternSize != patternSize - 1)
                        std__cout.Append(markerPatternDescriptor.data()[i]).Append(", ");
                    else
                        std__cout.Append(markerPatternDescriptor.data()[i]).Append("]").AppendLine();
                }
                std__cout.AppendLine();
                */

                /*
                std__cout.Append(recognizedPatternsDescriptors.getNbDescriptors()).Append(" recognized Pattern Descriptors ").AppendLine();
                uint desrciptorSize = recognizedPatternsDescriptors.getDescriptorByteSize();
                for (uint i = 0; i < recognizedPatternsDescriptors.getNbDescriptors() / 4; i++)
                {
                    for (int j = 0; j < patternSize; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            std__cout.Append("[");
                            for (int l = 0; l < patternSize; l++)
                            {
                                std__cout.Append(recognizedPatternsDescriptors.data()[desrciptorSize*((i*4)+k) + j*patternSize + l]);
                                if (l != patternSize - 1)
                                    std__cout.Append(", ");
                            }
                            std__cout.Append("]");
                        }
                        std__cout.AppendLine();
                    }
                    std__cout.AppendLine().AppendLine();
                }
                */

                /*
                std__cout.Append(recognizedContours.Count).Append(" Recognized Pattern contour ").AppendLine();
                for (int i = 0; i < recognizedContours.Count / 4; i++)
                {
                    for (int j = 0; j < recognizedContours[i].Count; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            std__cout.Append("[").Append(recognizedContours[i * 4 + k][j].getX()).Append(", ").Append(recognizedContours[i * 4 + k][j].getY()).Append("] ");
                        }
                        std__cout.AppendLine();
                    }
                    std__cout.AppendLine().AppendLine();
                }
                std__cout.AppendLine();
                */
#endif

                // From extracted squared binary pattern, match the one corresponding to the squared binary marker
                if (patternMatcher.match(markerPatternDescriptor, recognizedPatternsDescriptors, patternMatches) == Api.Features.RetCode.DESCRIPTORS_MATCHER_OK)
                {
#if !NDEBUG
                    std__cout.Append("Matches :").AppendLine();
                    for (int num_match = 0; num_match < patternMatches.Count; num_match++)
                        std__cout.Append("Match [").Append(patternMatches[num_match].getIndexInDescriptorA()).Append(",").Append(patternMatches[num_match].getIndexInDescriptorB()).Append("], dist = ").Append(patternMatches[num_match].getMatchingScore()).AppendLine();
                    std__cout.AppendLine().AppendLine();
#endif

                    // Reindex the pattern to create two vector of points, the first one corresponding to marker corner, the second one corresponding to the poitsn of the contour
                    patternReIndexer.reindex(recognizedContours, patternMatches, pattern2DPoints, img2DPoints).Check();
#if !NDEBUG
                    LOG_DEBUG("2D Matched points :");
                    for (int i = 0; i < img2DPoints.Count; i++)
                        LOG_DEBUG("{0}", img2DPoints[i]);
                    for (int i = 0; i < pattern2DPoints.Count; i++)
                        LOG_DEBUG("{0}", pattern2DPoints[i]);
                    overlay2DCircles.drawCircles(img2DPoints, inputImage);
#endif
                    // Compute the 3D position of each corner of the marker
                    img2worldMapper.map(pattern2DPoints, pattern3DPoints).Check();
#if !NDEBUG
                    std__cout.Append("3D Points position:").AppendLine();
                    for (int i = 0; i < pattern3DPoints.Count; i++)
                        LOG_DEBUG("{0}", pattern3DPoints[i]);
#endif
                    // Compute the pose of the camera using a Perspective n Points algorithm using only the 4 corners of the marker
                    if (PnP.estimate(img2DPoints, pattern3DPoints, pose) == FrameworkReturnCode._SUCCESS)
                    {
                        return FrameworkReturnCode._SUCCESS;
                    }
                }
#if !NDEBUG
                //LOG_DEBUG(std__cout.ToString());
                std__cout.Clear();
#endif
            }
            return FrameworkReturnCode._ERROR_;
        }

        // structures
        readonly Image greyImage;
        readonly Image binaryImage;

        readonly Contour2DfList contours;
        readonly Contour2DfList filtered_contours;
        readonly ImageList patches;
        readonly Contour2DfList recognizedContours;
        readonly DescriptorBuffer recognizedPatternsDescriptors;
        readonly DescriptorBuffer markerPatternDescriptor;
        readonly DescriptorMatchVector patternMatches;
        readonly Point2DfList pattern2DPoints;
        readonly Point2DfList img2DPoints;
        readonly Point3DfList pattern3DPoints;

        // components
        readonly IMarker2DSquaredBinary binaryMarker;

#if !NDEBUG
        readonly IImageViewer imageViewer;
        readonly IImageViewer imageViewerGrey;
        readonly IImageViewer imageViewerBinary;
        readonly IImageViewer imageViewerContours;
        readonly IImageViewer imageViewerFilteredContours;
#endif

        readonly IImageConvertor imageConvertor;
        readonly IImageFilter imageFilterBinary;
        readonly IContoursExtractor contoursExtractor;
        readonly IContoursFilter contoursFilter;
        readonly IPerspectiveController perspectiveController;
        readonly IDescriptorsExtractorSBPattern patternDescriptorExtractor;

        readonly IDescriptorMatcher patternMatcher;
        readonly ISBPatternReIndexer patternReIndexer;

        readonly IImage2WorldMapper img2worldMapper;
        readonly I3DTransformFinderFrom2D3D PnP;

#if !NDEBUG
        I2DOverlay overlay2DContours;
        I2DOverlay overlay2DCircles;
#endif
    }
}
