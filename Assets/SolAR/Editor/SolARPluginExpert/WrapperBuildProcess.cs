using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

namespace SolAR
{
    class WrapperBuildProcess : IPreprocessBuildWithReport
    {
        public PipelineManager pipelineMng = GameObject.Find("SolARPipelineLoader").GetComponent<PipelineManager>();

        public int callbackOrder
        {
            get
            {
                return 0;
            }
        }

        public void OnPreprocessBuild(BuildReport report)
        {   //DEPRECATED
            /*
            switch (report.summary.platform)
            {
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
                            sw.Write((res + Application.productName + "_Data" + tmpPath));
                        }
                        //Debug.Log(pipelineMng.conf.path);
                        break;
                    }
                case BuildTarget.StandaloneOSX:
                    break;
                case BuildTarget.Android:
                    break;
                case BuildTarget.iOS:
                    break;
            }
            */
        }
        private void ModifyPaths(string path, BuildReport report)
        {
            StreamReader input = new StreamReader(path);
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
                            break;
                        case BuildTarget.iOS:
                            break;
                    }
                    attribValue.SetValue(new_value);
                }
            }

            input.Close();
            doc.Save(path);
            return;
        }
    }
}

