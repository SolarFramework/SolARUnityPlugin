using System.IO;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using SolAR;

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
            Debug.LogWarning("[ANDROID] Configuration xml found in internal memory : " + xml+"\n It will be used for configuration");
        }
        else
        {
            Debug.LogWarning("[ANDROID] No configuration xml found in internal memory : " + old_xml+"\n "+xml+" will be used for configuration");
        }
        // Get configuration xml data
        if (!xml.Equals(old_xml))
        {
            //WWW request on .apk 
            UnityWebRequest www = new UnityWebRequest(xml);
            www.downloadHandler = new DownloadHandlerBuffer();
            UnityWebRequestAsyncOperation request = www.SendWebRequest();
            while (!request.isDone) { }

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("[ANDROID] WWW request on : " + xml + " : " + www.error+" cloning of resources canceled");
                www.downloadHandler.Dispose();
                return;
            }

            data = www.downloadHandler.text;
            www.downloadHandler.Dispose();

            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (data.Length>0 && data.StartsWith(_byteOrderMarkUtf8))
            {
                data = data.Remove(0, _byteOrderMarkUtf8.Length);
            }
        }
        else
        {
            //Read conf file in system storage
            StreamReader input = new StreamReader(xml);
            data = input.ReadToEnd();
            input.Close();
        }

        // Clone content in external directory with correct path
        CloneManager clone = new CloneManager();
        var doc = XDocument.Parse(data.ToString());
        var file = new[] {
            doc.Element("assets").Element("streamingAssets").Elements("file")
        };

        foreach (var f in file)
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
        Debug.Log("[ANDROID] REPLACE PATH USED FOR PLUGINS : "+ Application.dataPath.Replace("/base.apk", "/lib/arm64/"));
        StreamReader input = new StreamReader(filepath);
        var doc = XDocument.Parse(input.ReadToEnd());
        var module = doc.Element("xpcf-registry").Elements("module");

        foreach (var attribute in module.Attributes())
        {
            if (attribute.Name == "path")
            {
                attribute.SetValue(Application.dataPath.Replace("/base.apk", "/lib/arm64/"));
            }
        }
        input.Close();
        doc.Save(filepath);
        return;
    }

    /** <summary>
     * Write a cache with the path of the current pipeline used
     * </summary>
     * */
    public static void SaveConfiguration(string configurationPath)
    {
        string dest = Application.persistentDataPath+ "/StreamingAssets/SolAR/Android/.pipeline";
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
            StreamReader input = new StreamReader(dest);
            string data = input.ReadToEnd();
            input.Close();

            for(int i=0;i<pipeline.m_pipelinesPath.Length;i++)
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

    private class CloneManager
    {
        private List<Tuple<string, string>> m_data;
        public CloneManager()
        {
            m_data = new List<Tuple<string, string>>();
        }

        public void Add(string source, string dest)
        {
            m_data.Add(new Tuple<string, string>(source, dest));
        }

        public void Execute()
        {
            foreach (Tuple<string, string> c in m_data)
            {
                Clone(c.Item1, c.Item2);
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
            UnityWebRequest www = new UnityWebRequest(source);
            www.downloadHandler = new DownloadHandlerBuffer();
            UnityWebRequestAsyncOperation request = www.SendWebRequest();
            while (!request.isDone) { }

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

            foreach (Tuple<string, string> t in m_data)
            {
                tostring += Path.GetFileName(t.Item1.ToString()) + "\n";
            }
            tostring += "--CloneManager--\n";
            return tostring;
        }
    }
}
