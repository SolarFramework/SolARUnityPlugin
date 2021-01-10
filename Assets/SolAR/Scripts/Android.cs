using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using SolAR;
using UnityEngine;
using UnityEngine.Networking;

public class Android
{
    /** <summary>
     * Clone Unity StreamingAssets
     * </summary>
     * <remarks>
     * File to copy 
     * </remarks>
     * */
    public static void AndroidCloneResources(string xml)
    {
        string data;
        string old_xml = Application.persistentDataPath + xml.Replace(Application.streamingAssetsPath, "/StreamingAssets");

        // Check if conf xml already exist in internal storage
        if (File.Exists(old_xml))
        {
            xml = old_xml;
            Debug.LogWarning("[ANDROID] Configuration xml found in internal memory : " + xml + "\n It will be used for configuration");
        }
        else
        {
            Debug.LogWarning("[ANDROID] No configuration xml found in internal memory : " + old_xml + "\n " + xml + " will be used for configuration");
        }
        // Get configuration xml data
        if (!xml.Equals(old_xml))
        {
            //WWW request on .apk 
            var www = new UnityWebRequest(xml)
            {
                downloadHandler = new DownloadHandlerBuffer()
            };
            var request = www.SendWebRequest();
            while (!request.isDone) { } //120: NON, ca bloque le thread courant

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("[ANDROID] WWW request on : " + xml + " : " + www.error + " cloning of resources canceled");
                www.downloadHandler.Dispose();
                return;
            }

            data = www.downloadHandler.text;
            www.downloadHandler.Dispose();

            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (data.Length > 0 && data.StartsWith(_byteOrderMarkUtf8))
            {
                data = data.Remove(0, _byteOrderMarkUtf8.Length);
            }
        }
        else
        {
            //Read conf file in system storage
            data = File.ReadAllText(xml);
        }

        // Clone content in external directory with correct path
        var clone = new CloneManager();
        var doc = XDocument.Parse(data);
        //var files = new[] {
        //    doc.Element("assets").Element("streamingAssets").Elements("file")
        //};

        var f = doc.Element("assets").Element("streamingAssets").Elements("file");

        //foreach (var f in files)
        {
            foreach (var attribute in f.Attributes())
            {
                if (attribute.Name == "path")
                {
                    //update path for terminal
                    string src = "";
                    string output = "";

                    if (attribute.Value.Contains("StreamingAssets"))
                    {
                        src = attribute.Value.Replace("./assets/StreamingAssets", Application.streamingAssetsPath);
                        output = attribute.Value.Replace("./assets", Application.persistentDataPath);
                    }

                    if ((attribute.Parent.Attribute("overWrite") != null && attribute.Parent.Attribute("overWrite").Value.Equals("true")) || !File.Exists(output))
                    {
                        //Overwrite
                        clone.Add(src, output);
                    }
                }
            }
        }
        Debug.Log(clone);
        clone.Execute();
    }

    /** <summary>
     * Replace path of  pipeline configuration XML for "Plugins"
     * </summary>
     * <remarks>
     * Path are depending on the device and are initialized on running
     * </remarks>
     * */
    public static void ReplacePathToApp(string filepath)
    {
        Debug.Log("[ANDROID] REPLACE PATH USED FOR PLUGINS : " + Application.dataPath.Replace("/base.apk", "/lib/arm64/"));
        var data = File.ReadAllText(filepath);
        var doc = XDocument.Parse(data);
        var module = doc.Element("xpcf-registry").Elements("module");

        foreach (var attribute in module.Attributes())
        {
            if (attribute.Name == "path")
            {
                attribute.SetValue(Application.dataPath.Replace("/base.apk", "/lib/arm64/"));
            }
        }
        doc.Save(filepath);
    }

    /** <summary>
     * Write a cache with the path of the current pipeline used
     * </summary>
     * */
    public static void SaveConfiguration(string configurationPath)
    {
        string dest = Application.persistentDataPath + "/StreamingAssets/SolAR/Android/.pipeline";
        if (File.Exists(dest)) File.Delete(dest);
        File.WriteAllText(dest, configurationPath);
    }

    /** <summary>
     * If exist load a configuration cache file containing path of the last pipeline used
     * </summary>
     * */
    public static void LoadConfiguration(SolARPipeline pipeline)
    {
        string dest = Application.persistentDataPath + "/StreamingAssets/SolAR/Android/.pipeline";
        if (File.Exists(dest))
        {
            string data = File.ReadAllText(dest);

            for (int i = 0; i < pipeline.m_pipelinesPath.Length; i++)
            {
                if (pipeline.m_pipelinesPath[i].Equals(data))
                {
                    pipeline.m_configurationPath = pipeline.m_pipelinesPath[i];
                    pipeline.m_uuid = pipeline.m_pipelinesUUID[i];
                    pipeline.m_selectedPipeline = i;
                }
            }
        }
    }

    class CloneManager
    {
        readonly List<Tuple<string, string>> m_data;

        public CloneManager()
        {
            m_data = new List<Tuple<string, string>>();
        }

        public void Add(string source, string dest)
        {
            m_data.Add(Tuple.Create(source, dest));
        }

        public void Execute()
        {
            foreach (var tuple in m_data)
            {
                Clone(tuple.Item1, tuple.Item2);
            }
            m_data.Clear();
        }

        private void Clone(string source, string dest)
        {
            //Create directory
            if (!Directory.Exists(Path.GetDirectoryName(dest)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dest));
            }

            //WWW request
            var www = new UnityWebRequest(source)
            {
                downloadHandler = new DownloadHandlerBuffer()
            };
            var request = www.SendWebRequest();
            while (!request.isDone) { } //120 NON

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("CloneFile error - " + source + " / " + www.error);
                www.downloadHandler.Dispose();
                return;
            }
            else
            {
                if (File.Exists(dest)) File.Delete(dest);
                File.WriteAllBytes(dest, www.downloadHandler.data);
            }
            www.downloadHandler.Dispose();
        }

        public override string ToString()
        {
            string tostring = "--CloneManager-- \nCount: " + m_data.Count + "\n";

            foreach (var tuple in m_data)
            {
                tostring += Path.GetFileName(tuple.Item1) + "\n";
            }
            tostring += "--CloneManager--\n";
            return tostring;
        }
    }
}
