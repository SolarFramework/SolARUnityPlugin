using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using System.IO;

namespace SolAR {

    class SolARBuildProcess : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        private List<string> createdStreamingAssetsFolders = new List<string>();

        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            SolARPipelineLoader[] solARPipelineLoaders = (SolARPipelineLoader[])GameObject.FindObjectsOfType<SolARPipelineLoader>();
            foreach (SolARPipelineLoader solarPipelineLoader in solARPipelineLoaders)
            {
                switch (report.summary.platform)
                {
                    case BuildTarget.StandaloneWindows:
                    case BuildTarget.StandaloneWindows64:
                        {
                            string windowsPipelineConfPath = solarPipelineLoader.m_configurationPath;
                            // Create a directory in the streamingAssets folder to copy the pipeline configuration files
                            if (!Directory.Exists(Path.GetDirectoryName(Application.streamingAssetsPath + solarPipelineLoader.m_configurationPath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(Application.streamingAssetsPath + solarPipelineLoader.m_configurationPath));
                                // Store the folders to remove it after the build process
                                createdStreamingAssetsFolders.Add(Path.GetDirectoryName(Application.streamingAssetsPath + solarPipelineLoader.m_configurationPath));
                            }
                            // If there is no pipeline configuration file specific for a given platform (put in a dedicated folder such as StandaloneWindows), move the pipeline used in editor mode to the streamingAssetsFolder
                            windowsPipelineConfPath = windowsPipelineConfPath.Insert(windowsPipelineConfPath.LastIndexOf("/") + 1, "StandaloneWindows/");
                            if (!System.IO.File.Exists(Application.dataPath + windowsPipelineConfPath))
                            {
<<<<<<< HEAD:Assets/SolAR/Editor/SolARPluginNovice/SolARBuildProcess.cs
                                if (File.Exists(Application.streamingAssetsPath + pipeline.m_configurationPath)) File.Delete(Application.streamingAssetsPath + pipeline.m_configurationPath);
                                FileUtil.CopyFileOrDirectory(Application.dataPath + pipeline.m_configurationPath, Application.streamingAssetsPath + pipeline.m_configurationPath);
=======
                                FileUtil.CopyFileOrDirectory(Application.dataPath + solarPipelineLoader.m_configurationPath, Application.streamingAssetsPath + solarPipelineLoader.m_configurationPath);
>>>>>>> develop:Assets/SolAR/Editor/SolARBuildProcess.cs
                                // Update in the pipeline configuration file the path for plugins and configuration property related to a path to reference them according to the executable folder 
                                ReplacePluginPaths(Application.streamingAssetsPath + solarPipelineLoader.m_configurationPath, report);
                            }
                            // If there is a pipeline configuration file specific for a given platform (put in a dedicated folder such as StandaloneWindows), move it to the streamingAssets folder
                            else
                            {
                                FileUtil.CopyFileOrDirectory(Application.dataPath + windowsPipelineConfPath, Application.streamingAssetsPath + solarPipelineLoader.m_configurationPath);
                            }
                            break;
                        }
                    case BuildTarget.StandaloneOSX:
                        break;
                    case BuildTarget.Android:
                        break;
                    case BuildTarget.iOS:
                        break;
                }
            }
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            // Remove directories create before the built process used to store pipeline configuration files relative to the streamingAssets folder
            foreach (var path in createdStreamingAssetsFolders)
            {
                Directory.Delete(path, true);
            }
            createdStreamingAssetsFolders.Clear();
        }

        private void ReplacePluginPaths(string confFileName, BuildReport report)
        {
            StreamReader input = new StreamReader(confFileName);
            var doc = XDocument.Parse(input.ReadToEnd());

            var module = doc.Element("xpcf-registry").Elements("module");
            foreach (var attribute in module.Attributes())
            {
                if (attribute.Name == "path")
                {
                    if (attribute.Value.Contains("Plugins"))
                    {
                        string new_value = attribute.Value;
                        new_value = attribute.Value.Substring(attribute.Value.IndexOf("Plugins"));
                        switch (report.summary.platform)
                        {
                            case BuildTarget.StandaloneWindows:
                            case BuildTarget.StandaloneWindows64:
                                // For windows, during the built process plugins dll are copied from the Assets/plugins folder to the productname_Data/Plugins folder.
                                new_value = "./" + Application.productName + "_Data/Plugins";
                                break;
                            case BuildTarget.StandaloneOSX:
                                break;
                            case BuildTarget.Android:
                                break;
                            case BuildTarget.iOS:
                                break;
                        }
                        attribute.SetValue(new_value);
                    }
                }
            }
            var configComp = doc.Element("xpcf-registry").Elements("configuration").Elements("component");
            foreach (var element in configComp.Elements("property"))
            {
                var attriName = element.Attribute("name");
                if (attriName.Value.Contains("File") || attriName.Value.Contains("Path") || attriName.Value.Contains("file") || attriName.Value.Contains("path"))
                {
                    var attribValue = element.Attribute("value");
                    string new_value = "";
                    switch (report.summary.platform)
                    {
                        case BuildTarget.StandaloneWindows:
                        case BuildTarget.StandaloneWindows64:
                            // For windows, during the built process, streamingAssets folder is copied from the Assets/streamingAssets to the productname_Data/streamingAssets folder.
                            new_value = attribValue.Value.Replace("./Assets/", "./" + Application.productName + "_Data/");
                            break;
                        case BuildTarget.StandaloneOSX:
                            break;
                        case BuildTarget.Android:
                            break;
                        case BuildTarget.iOS:
                            break;
                    }
                    attribValue.SetValue(new_value);
                }
            }

            input.Close();
            doc.Save(confFileName);
            return;
        }
    }
}
