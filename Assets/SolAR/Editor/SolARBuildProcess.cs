using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SolAR
{
    class SolARBuildProcess : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        readonly List<string> createdStreamingAssetsFolders = new List<string>();

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            var solARPipelineLoaders = Object.FindObjectsOfType<SolARPipeline>();
            foreach (var pipeline in solARPipelineLoaders)
            {
                foreach (string conf in pipeline.m_pipelinesPath)
                {
                    var path = Application.streamingAssetsPath + conf;
                    var name = Path.GetDirectoryName(path);
                    switch (report.summary.platform)
                    {
                        case BuildTarget.StandaloneWindows:
                        case BuildTarget.StandaloneWindows64:
                            {
                                string windowsPipelineConfPath = conf;
                                // Create a directory in the streamingAssets folder to copy the pipeline configuration files
                                if (!Directory.Exists(name))
                                {
                                    Directory.CreateDirectory(name);
                                    // Store the folders to remove it after the build process
                                    createdStreamingAssetsFolders.Add(name);
                                }
                                // If there is no pipeline configuration file specific for a given platform (put in a dedicated folder such as StandaloneWindows), move the pipeline used in editor mode to the streamingAssetsFolder
                                windowsPipelineConfPath = windowsPipelineConfPath.Insert(windowsPipelineConfPath.LastIndexOf("/") + 1, "StandaloneWindows/");
                                if (!File.Exists(Application.dataPath + windowsPipelineConfPath))
                                {
                                    if (File.Exists(path)) File.Delete(path);
                                    FileUtil.CopyFileOrDirectory(Application.dataPath + conf, path);
                                    // Update in the pipeline configuration file the path for plugins and configuration property related to a path to reference them according to the executable folder 
                                    ReplacePluginPaths(path, report);
                                }
                                // If there is a pipeline configuration file specific for a given platform (put in a dedicated folder such as StandaloneWindows), move it to the streamingAssets folder
                                else
                                {
                                    FileUtil.CopyFileOrDirectory(Application.dataPath + windowsPipelineConfPath, path);
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
                            if (!Directory.Exists(name))
                            {
                                Directory.CreateDirectory(name);
                                // Store the folders to remove it after the build process
                                createdStreamingAssetsFolders.Add(name);
                            }
                            // If there is no pipeline configuration file specific for a given platform (put in a dedicated folder such as StandaloneWindows), move the pipeline used in editor mode to the streamingAssetsFolder
                            if (File.Exists(path)) File.Delete(path);
                            FileUtil.CopyFileOrDirectory(Application.dataPath + conf, path);
                            // Update in the pipeline configuration file the path for plugins and configuration property related to a path to reference them according to the executable folder 
                            ReplacePluginPaths(path, report);
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

        void ReplacePluginPaths(string confFileName, BuildReport report)
        {
            string androidPersistentPath = "/storage/emulated/0/Android/data/" + Application.identifier + "/files";
            var doc = XDocument.Parse(File.ReadAllText(confFileName));

            var module = doc.Element("xpcf-registry").Elements("module");
            foreach (var attribute in module.Attributes())
            {
                if (attribute.Name == "path")
                {
                    if (attribute.Value.Contains("Plugins"))
                    {
                        string new_value = attribute.Value;
                        new_value = new_value.Substring(new_value.IndexOf("Plugins"));
                        switch (report.summary.platform)
                        {
                            // For Windows, during the built process plugins dll are copied from the Assets/plugins folder to the productname_Data/Plugins folder.
                            case BuildTarget.StandaloneWindows:
                                new_value = "./" + Application.productName + "_Data/Plugins/x86";
                                break;
                            case BuildTarget.StandaloneWindows64:
                                new_value = "./" + Application.productName + "_Data/Plugins/x86_64";
                                break;
                            case BuildTarget.StandaloneOSX:
                                break;
                            case BuildTarget.Android:
                                // For Android, plugins are included in private app directory value can only be set on running by Android.ReplacePathToApp 
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
                if (attriName.Value.Contains("File")
                    || attriName.Value.Contains("Path")
                    || attriName.Value.Contains("file")
                    || attriName.Value.Contains("path")
                    || attriName.Value.Contains("directory"))
                {
                    var attribValue = element.Attribute("value");
                    string new_value = "";
                    switch (report.summary.platform)
                    {
                        case BuildTarget.StandaloneWindows:
                        case BuildTarget.StandaloneWindows64:
                            // For Windows, during the built process, streamingAssets folder is copied from the Assets/streamingAssets to the productname_Data/streamingAssets folder.
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
            doc.Save(confFileName);
        }

        /// <summary>
        /// Fill an xml with resources inside ./Assets/StreamingAssets/*
        /// </summary>
        /// <param name="output">Path to write to xml</param>
        void BuildAndroidXML(string output)
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
                        if (overWriteStreamingAssets == true) element.Add(new XAttribute("overWrite", overWriteStreamingAssets.ToString().ToLower()));
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
