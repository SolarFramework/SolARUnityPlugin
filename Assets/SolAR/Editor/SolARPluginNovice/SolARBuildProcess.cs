using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using System.IO;

namespace SolAR
{

    class SolARBuildProcess : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        private List<string> createdStreamingAssetsFolders = new List<string>();

        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            SolARPipeline[] solARPipelineLoaders = (SolARPipeline[])GameObject.FindObjectsOfType<SolARPipeline>();
            foreach (SolARPipeline pipeline in solARPipelineLoaders)
            {
                foreach (string conf in pipeline.m_pipelinesPath)
                {
                    switch (report.summary.platform)
                    {
                        case BuildTarget.StandaloneWindows:
                        case BuildTarget.StandaloneWindows64:
                            {
                                string windowsPipelineConfPath = conf;
                                // Create a directory in the streamingAssets folder to copy the pipeline configuration files
                                if (!Directory.Exists(Path.GetDirectoryName(Application.streamingAssetsPath + conf)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(Application.streamingAssetsPath + conf));
                                    // Store the folders to remove it after the build process
                                    createdStreamingAssetsFolders.Add(Path.GetDirectoryName(Application.streamingAssetsPath + conf));
                                }
                                // If there is no pipeline configuration file specific for a given platform (put in a dedicated folder such as StandaloneWindows), move the pipeline used in editor mode to the streamingAssetsFolder
                                windowsPipelineConfPath = windowsPipelineConfPath.Insert(windowsPipelineConfPath.LastIndexOf("/") + 1, "StandaloneWindows/");
                                if (!System.IO.File.Exists(Application.dataPath + windowsPipelineConfPath))
                                {
                                    if (File.Exists(Application.streamingAssetsPath + conf)) File.Delete(Application.streamingAssetsPath + conf);
                                    FileUtil.CopyFileOrDirectory(Application.dataPath + conf, Application.streamingAssetsPath + conf);
                                    // Update in the pipeline configuration file the path for plugins and configuration property related to a path to reference them according to the executable folder 
                                    ReplacePluginPaths(Application.streamingAssetsPath + conf, report);
                                }
                                // If there is a pipeline configuration file specific for a given platform (put in a dedicated folder such as StandaloneWindows), move it to the streamingAssets folder
                                else
                                {
                                    FileUtil.CopyFileOrDirectory(Application.dataPath + windowsPipelineConfPath, Application.streamingAssetsPath + conf);
                                }
                                break;
                            }
                        case BuildTarget.StandaloneOSX:
                            break;
                        case BuildTarget.Android:
                            BuildAndroidXML(Application.streamingAssetsPath + "/SolAR/Android/android.xml");
                            //Android build clone content of Assets/StreamingAssets/ in assets/ in the .apk archive
                            // Pipelines
                            // Create a directory in the streamingAssets folder to copy the pipeline configuration files
                            if (!Directory.Exists(Path.GetDirectoryName(Application.streamingAssetsPath + conf)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(Application.streamingAssetsPath + conf));
                                // Store the folders to remove it after the build process
                                createdStreamingAssetsFolders.Add(Path.GetDirectoryName(Application.streamingAssetsPath + conf));
                            }
                            // If there is no pipeline configuration file specific for a given platform (put in a dedicated folder such as StandaloneWindows), move the pipeline used in editor mode to the streamingAssetsFolder
                            if (File.Exists(Application.streamingAssetsPath + conf)) File.Delete(Application.streamingAssetsPath + conf);
                            FileUtil.CopyFileOrDirectory(Application.dataPath + conf, Application.streamingAssetsPath + conf);
                            // Update in the pipeline configuration file the path for plugins and configuration property related to a path to reference them according to the executable folder 
                            ReplacePluginPaths(Application.streamingAssetsPath + conf, report);
                            break;
                        case BuildTarget.iOS:
                            break;
                    }
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
            string androidPersistentPath = "/storage/emulated/0/Android/data/"+Application.identifier+ "/files";
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
                                // For Android , plugins are included in private app directory value can only be set on running by Android.ReplacePathToApp 
                                break;
                            case BuildTarget.iOS:
                                break;
                        }
                        attribute.SetValue(new_value);
                    }
                }
            }
            var configComp = doc.Element("xpcf-registry").Elements("properties").Elements("configure");
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
                            // For Android, during the built process, an xml is build by SolARBuildProcess.BuildAndroidXML
                            // the content of this xml will be clone from private app directory to external app directory (androidPersistentPath => /storage/emulated/0/Android/data/com.bcom.SolARUnityPlugin/files/)
                            new_value = attribValue.Value.Replace("./Assets", androidPersistentPath);
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

        /// <summary>
        /// Fill an xml with resources inside ./Assets/StreamingAssets/*
        /// </summary>
        /// <param name="output">Path to write to xml</param>
        private void BuildAndroidXML(string output)
        {
            XDocument doc = new XDocument();
            //Streaming Assets
            XElement streamingAssets = new XElement("streamingAssets");
            bool overWriteStreamingAssets;
            FileInfo[] info;
            string comment = "";
            foreach (DirectoryInfo dir in new DirectoryInfo(Application.streamingAssetsPath + "/SolAR/").GetDirectories("*.*", SearchOption.AllDirectories))
            {
                info = dir.GetFiles("*.*");
                foreach (FileInfo f in info)
                {
                    overWriteStreamingAssets = true;
                    if (!f.Extension.Equals(".meta"))
                    {
                        if (!comment.Equals(f.Directory.Name))
                        {
                            comment = f.Directory.Name;
                            XComment c = new XComment(comment);
                            streamingAssets.Add(c);
                        }
                        if (f.Name.Equals("android.xml"))
                        {
                            streamingAssets.Add(new XComment("/!\\ Don't use overWrite attribute for configuration, if you want to use the configuration xml in the .apk delete this configuration xml otherwise it will use this one"));
                            overWriteStreamingAssets = false;
                        }
                        XElement element = new XElement("file");
                        element.Add(new XAttribute("path", f.FullName.Replace("\\", "/").Replace(Application.dataPath, "./assets")));
                        if(overWriteStreamingAssets==true) element.Add(new XAttribute("overWrite", overWriteStreamingAssets.ToString().ToLower()));
                        streamingAssets.Add(element);
                    }
                }
            }

            //Merge
            XElement assets = new XElement("assets");
            assets.Add(streamingAssets);
            doc.Add(assets);
            doc.Save(output);
        }
    }
}
