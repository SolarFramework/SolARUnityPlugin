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

#define NDEBUG

using SolAR.Api.Features;
using SolAR.Api.Geom;
using SolAR.Api.Image;
using SolAR.Api.Input.Devices;
using SolAR.Api.Input.Files;
using SolAR.Api.Solver.Pose;
using SolAR.Core;
using SolAR.Datastructure;
using UniRx;
using XPCF.Api;

#if !NDEBUG
using System.Runtime.InteropServices;
using SolAR.Api.Display;
#endif

namespace SolAR.Samples
{
    public class FiducialPipeline : AbstractPipeline
    {
        const int MIN_THRESHOLD = -1;
        const int MAX_THRESHOLD = 220;
        const float NB_THRESHOLD = 8;

        public FiducialPipeline(IComponentManager xpcfComponentManager) : base(xpcfComponentManager)
        {
            // declare and create components
            LOG_INFO("Start creating components");

            //auto camera =xpcfComponentManager->resolve<input::devices::ICamera>();
            binaryMarker = Resolve<IMarker2DSquaredBinary>();

#if !NDEBUG
            //imageViewer = Resolve<IImageViewer>("original");
            imageViewerGrey = Resolve<IImageViewer>("grey");
            imageViewerBinary = Resolve<IImageViewer>("binary");
            imageViewerContours = Resolve<IImageViewer>("contours");
            imageViewerFilteredContours = Resolve<IImageViewer>("filteredContours");
#endif

            imageFilterBinary = Resolve<IImageFilter>();
            imageConvertor = Resolve<IImageConvertor>();
            contoursExtractor = Resolve<IContoursExtractor>();
            contoursFilter = Resolve<IContoursFilter>();
            perspectiveController = Resolve<IPerspectiveController>();
            patternDescriptorExtractor = Resolve<IDescriptorsExtractorSBPattern>();

            patternMatcher = Resolve<IDescriptorMatcher>();
            patternReIndexer = Resolve<ISBPatternReIndexer>();

            img2worldMapper = Resolve<IImage2WorldMapper>();
            PnP = Resolve<I3DTransformFinderFrom2D3D>();
            //overlay3D = Resolve<I3DOverlay>();
#if !NDEBUG
            overlay2DContours = Resolve<I2DOverlay>("contours");
            overlay2DCircles = Resolve<I2DOverlay>("circles");
#endif

            //SRef<Image> inputImage;
            greyImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            binaryImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);
#if !NDEBUG
            contoursImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);
            filteredContoursImage = SharedPtr.Alloc<Image>().AddTo(subscriptions);
#endif

            contours = new Contour2DfArray().AddTo(subscriptions);
            filtered_contours = new Contour2DfArray().AddTo(subscriptions);
            patches = new ImageList().AddTo(subscriptions);
            recognizedContours = new Contour2DfArray().AddTo(subscriptions);
            recognizedPatternsDescriptors = SharedPtr.Alloc<DescriptorBuffer>().AddTo(subscriptions);
            markerPatternDescriptor = SharedPtr.Alloc<DescriptorBuffer>().AddTo(subscriptions);
            patternMatches = new DescriptorMatchVector().AddTo(subscriptions);
            pattern2DPoints = new Point2DfArray().AddTo(subscriptions);
            img2DPoints = new Point2DfArray().AddTo(subscriptions);
            pattern3DPoints = new Point3DfArray().AddTo(subscriptions);
            //Transform3Df                         pose;

            //CamCalibration K;

            // components initialisation
            binaryMarker.loadMarker().Check();
            patternDescriptorExtractor.extract(binaryMarker.getPattern(), markerPatternDescriptor).Check();
            var binaryMarkerSize = binaryMarker.getSize();

            LOG_DEBUG("Marker pattern:\n {0}", binaryMarker.getPattern().getPatternMatrix());

            // Set the size of the box to display according to the marker size in world unit
            //var overlay3D_size = overlay3D.BindTo<IConfigurable>().getProperty("size");
            //overlay3D_size.setFloatingValue(binaryMarkerSize.width, 0);
            //overlay3D_size.setFloatingValue(binaryMarkerSize.height, 1);
            //overlay3D_size.setFloatingValue(binaryMarkerSize.height / 2.0f, 2);


            patternSize = binaryMarker.getPattern().getSize();

            patternDescriptorExtractor.BindTo<IConfigurable>().getProperty("patternSize").setIntegerValue(patternSize);
            patternReIndexer.BindTo<IConfigurable>().getProperty("sbPatternSize").setIntegerValue(patternSize);

            // NOT WORKING ! initialize image mapper with the reference image size and marker size
            var img2worldMapperConf = img2worldMapper.BindTo<IConfigurable>();
            img2worldMapperConf.getProperty("digitalWidth").setIntegerValue(patternSize);
            img2worldMapperConf.getProperty("digitalHeight").setIntegerValue(patternSize);
            img2worldMapperConf.getProperty("worldWidth").setFloatingValue(binaryMarkerSize.width);
            img2worldMapperConf.getProperty("worldHeight").setFloatingValue(binaryMarkerSize.height);
        }

        public override void SetCameraParameters(Matrix3x3f intrinsics, Vector5f distortion)
        {
            PnP.setCameraParameters(intrinsics, distortion);
            //overlay3D.setCameraParameters(intrinsics, distortion);
        }

        public override FrameworkReturnCode Proceed(Image inputImage, Transform3Df pose, ICamera camera)
        {
            // Convert Image from RGB to grey
            imageConvertor.convert(inputImage, greyImage, Image.ImageLayout.LAYOUT_GREY).Check();

            var imageFilterBinaryConf = imageFilterBinary.BindTo<IConfigurable>();

            bool marker_found = false;
            for (int num_threshold = 0; !marker_found && num_threshold < NB_THRESHOLD; num_threshold++)
            {
                // Compute the current Threshold valu for image binarization
                int threshold = UnityEngine.Mathf.RoundToInt(MIN_THRESHOLD + (MAX_THRESHOLD - MIN_THRESHOLD) * (num_threshold / (NB_THRESHOLD - 1)));

                // Convert Image from grey to black and white
                imageFilterBinaryConf.getProperty("min").setIntegerValue(threshold);
                imageFilterBinaryConf.getProperty("max").setIntegerValue(255);

                // Convert Image from grey to black and white
                imageFilterBinary.filter(greyImage, binaryImage).Check();

                // Extract contours from binary image
                contoursExtractor.extract(binaryImage, contours).Check();
#if !NDEBUG
                contoursImage = binaryImage.copy();
                overlay2DContours.drawContours(contours, contoursImage);
#endif
                // Filter 4 edges contours to find those candidate for marker contours
                contoursFilter.filter(contours, filtered_contours).Check();

#if !NDEBUG
                filteredContoursImage = binaryImage.copy();
                overlay2DContours.drawContours(filtered_contours, filteredContoursImage);
#endif
                // Create one warpped and cropped image by contour
                perspectiveController.correct(binaryImage, filtered_contours, patches).Check();

                // test if this last image is really a squared binary marker, and if it is the case, extract its descriptor
                if (patternDescriptorExtractor.extract(patches, filtered_contours, recognizedPatternsDescriptors, recognizedContours) == FrameworkReturnCode._SUCCESS)
                {

#if !NDEBUG
                    var std__cout = new System.Text.StringBuilder();

                    std__cout.Append("Looking for the following descriptor:");
                    var markerDataPtr = markerPatternDescriptor.data();
                    for (int i = 0; i < markerPatternDescriptor.getNbDescriptors() * markerPatternDescriptor.getDescriptorByteSize(); i++)
                    {
                        var b = Marshal.ReadByte(markerDataPtr, i);
                        if (i % patternSize == 0)
                            std__cout.Append("[");
                        if (i % patternSize != patternSize - 1)
                            std__cout.Append(b).Append(", ");
                        else
                            std__cout.Append(b).Append("]").AppendLine();
                    }
                    std__cout.AppendLine();
                    // 

                    ///*
                    std__cout.Append(recognizedPatternsDescriptors.getNbDescriptors()).Append(" recognized Pattern Descriptors ").AppendLine();
                    int desrciptorSize = (int)recognizedPatternsDescriptors.getDescriptorByteSize();
                    var recognizedDataPtr = recognizedPatternsDescriptors.data();
                    for (int i = 0; i < recognizedPatternsDescriptors.getNbDescriptors() / 4; i++)
                    {
                        for (int j = 0; j < patternSize; j++)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                std__cout.Append("[");
                                for (int l = 0; l < patternSize; l++)
                                {
                                    var b = Marshal.ReadByte(recognizedDataPtr, desrciptorSize * ((i * 4) + k) + j * patternSize + l);
                                    std__cout.Append(b);
                                    if (l != patternSize - 1)
                                        std__cout.Append(", ");
                                }
                                std__cout.Append("]");
                            }
                            std__cout.AppendLine();
                        }
                        std__cout.AppendLine().AppendLine();
                    }
                    // */

                    ///*
                    std__cout.Append(recognizedContours.Count).Append(" Recognized Pattern contour ").AppendLine();
                    for (int i = 0; i < recognizedContours.Count / 4; i++)
                    {
                        for (int j = 0; j < recognizedContours[0].Count; j++)
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
                    // */
#endif

                    // From extracted squared binary pattern, match the one corresponding to the squared binary marker
                    if (patternMatcher.match(markerPatternDescriptor, recognizedPatternsDescriptors, patternMatches) == Api.Features.IDescriptorMatcher.RetCode.DESCRIPTORS_MATCHER_OK)
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
                            //LOG_DEBUG("Camera pose : \n {}", pose.matrix());
                            // Display a 3D box over the marker
                            //overlay3D.draw(pose, inputImage);
                            marker_found = true;
                        }
                    }
#if !NDEBUG
                    LOG_DEBUG(std__cout.ToString());
                    std__cout.Clear();
#endif
                }
            }
#if !NDEBUG
            // display images in viewers
            if (false
                //|| (imageViewer.display(inputImage) == FrameworkReturnCode._STOP)
                || (imageViewerGrey.display(greyImage) == FrameworkReturnCode._STOP)
                || (imageViewerBinary.display(binaryImage) == FrameworkReturnCode._STOP)
                || (imageViewerContours.display(contoursImage) == FrameworkReturnCode._STOP)
                || (imageViewerFilteredContours.display(filteredContoursImage) == FrameworkReturnCode._STOP)
            )
            {
                return FrameworkReturnCode._STOP;
            }
#endif
            return marker_found ? FrameworkReturnCode._SUCCESS : FrameworkReturnCode._ERROR_;
        }

        public override Sizef GetMarkerSize() => binaryMarker.getSize();

        // components
        readonly IMarker2DSquaredBinary binaryMarker;

#if !NDEBUG
        //readonly IImageViewer imageViewer;
        readonly IImageViewer imageViewerGrey;
        readonly IImageViewer imageViewerBinary;
        readonly IImageViewer imageViewerContours;
        readonly IImageViewer imageViewerFilteredContours;
#endif

        readonly IImageFilter imageFilterBinary;
        readonly IImageConvertor imageConvertor;
        readonly IContoursExtractor contoursExtractor;
        readonly IContoursFilter contoursFilter;
        readonly IPerspectiveController perspectiveController;
        readonly IDescriptorsExtractorSBPattern patternDescriptorExtractor;

        readonly IDescriptorMatcher patternMatcher;
        readonly ISBPatternReIndexer patternReIndexer;

        readonly IImage2WorldMapper img2worldMapper;
        readonly I3DTransformFinderFrom2D3D PnP;
        //readonly I3DOverlay overlay3D;
#if !NDEBUG
        readonly I2DOverlay overlay2DContours;
        readonly I2DOverlay overlay2DCircles;
#endif

        // structures
        readonly Image greyImage;
        readonly Image binaryImage;
#if !NDEBUG
        Image contoursImage;
        Image filteredContoursImage;
#endif

        readonly Contour2DfArray contours;
        readonly Contour2DfArray filtered_contours;
        readonly ImageList patches;
        readonly Contour2DfArray recognizedContours;
        readonly DescriptorBuffer recognizedPatternsDescriptors;
        readonly DescriptorBuffer markerPatternDescriptor;
        readonly DescriptorMatchVector patternMatches;
        readonly Point2DfArray pattern2DPoints;
        readonly Point2DfArray img2DPoints;
        readonly Point3DfArray pattern3DPoints;

        // others
        readonly int patternSize;
    }
}
