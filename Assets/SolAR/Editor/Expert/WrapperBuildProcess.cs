using System;
using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SolAR.Expert
{
    [Obsolete]
    class WrapperBuildProcess : IPreprocessBuildWithReport
    {
        public PipelineManager pipelineMng = UnityEngine.Object.FindObjectOfType<PipelineManager>();

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            //DEPRECATED
            /*
            switch (report.summary.platform)
            {
                default:
                case BuildTarget.StandaloneOSX:
                case BuildTarget.Android:
                case BuildTarget.iOS:
                    break;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    {
                        ModifyPaths(pipelineMng.conf.path, report);
                        int indexFirstSlash = pipelineMng.conf.path.IndexOf('\\');
                        string tmpPath = pipelineMng.conf.path.Substring(indexFirstSlash).Replace('\\', '/');
                        string test = report.summary.outputPath.Substring(report.summary.outputPath.LastIndexOf('/'));
                        string res = report.summary.outputPath.Substring(0, report.summary.outputPath.Length - test.Length) + '/';

                        using (StreamWriter sw = File.CreateText(res + "/confPath.txt"))
                        {
                            sw.Write(res + Application.productName + "_Data" + tmpPath);
                        }
                        //Debug.Log(pipelineMng.conf.path);
                        break;
                    }
            }
            // */
        }

        static void ModifyPaths(string path, BuildReport report)
        {
            var content = File.ReadAllText(path);
            XDocument doc = XDocument.Parse(content);

            bool hasChanged = false;
            var modulesPath = doc
                .Element("xpcf-registry")
                .Elements("module")
                .Attributes("path");
            foreach (var attribute in modulesPath)
            {
                var value = attribute.Value;
                var index = value.IndexOf("Plugins");
                if (index >= 0)
                {
                    value = value.Substring(index);
                    switch (report.summary.platform)
                    {
                        default:
                        case BuildTarget.StandaloneOSX:
                        case BuildTarget.Android:
                        case BuildTarget.iOS:
                            break;
                        // For windows, during the built process plugins dll are copied from the Assets/plugins folder to the productname_Data/Plugins folder.
                        case BuildTarget.StandaloneWindows:
                            value = "./" + Application.productName + "_Data/Plugins/x86";
                            break;
                        case BuildTarget.StandaloneWindows64:
                            value = "./" + Application.productName + "_Data/Plugins/x86_64";
                            break;
                    }
                    attribute.SetValue(value);
                    hasChanged = true;
                }
            }
            var configProps = doc
                .Element("xpcf-registry")
                .Element("properties")
                .Elements("configure")
                .Elements("property");
            foreach (var element in configProps)
            {
                var name = element.Attribute("name").Value;
                if (name.IndexOf("file", StringComparison.InvariantCultureIgnoreCase) >= 0
                    || name.IndexOf("path", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    var attribValue = element.Attribute("value");
                    string value;
                    switch (report.summary.platform)
                    {
                        default:
                        case BuildTarget.StandaloneOSX:
                        case BuildTarget.Android:
                        case BuildTarget.iOS:
                            value = "";
                            break;
                        case BuildTarget.StandaloneWindows:
                        case BuildTarget.StandaloneWindows64:
                            // For windows, during the built process, streamingAssets folder is copied from the Assets/streamingAssets to the productname_Data/streamingAssets folder.
                            value = attribValue.Value.Replace("./Assets/", "./" + Application.productName + "_Data/");
                            break;
                    }
                    attribValue.SetValue(value);
                    hasChanged = true;
                }
            }

            if (hasChanged) doc.Save(path);
        }
    }
}
