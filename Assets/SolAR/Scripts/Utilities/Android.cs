using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace SolAR
{
    public static class Android
    {
        /** <summary>
         * Clone Unity StreamingAssets
         * </summary>
         * <remarks>
         * File to copy 
         * </remarks>
         * */
        async public static Task AndroidCloneResources(string xml)
        {
            // Check if conf xml already exist in internal storage
            string old_xml = Application.persistentDataPath + xml.Replace(Application.streamingAssetsPath, "/StreamingAssets");
            if (File.Exists(old_xml))
            {
                xml = old_xml;
                Debug.LogWarningFormat("[ANDROID] Configuration xml found in internal memory : {0}\n It will be used for configuration", xml);
            }
            else
            {
                Debug.LogWarningFormat("[ANDROID] No configuration xml found in internal memory : {0}\n {1} will be used for configuration", old_xml, xml);
            }

            // Get configuration xml data
            string data;
            if (xml.Equals(old_xml))
            {
                //Read conf file in system storage
                data = File.ReadAllText(xml);
            }
            else
            {
                //WWW request on .apk 
                using (var downloadHandler = new DownloadHandlerBuffer())
                using (var www = new UnityWebRequest(xml) { downloadHandler = downloadHandler })
                {
                    var request = www.SendWebRequest();
                    while (!request.isDone) { await Task.Yield(); }

                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.LogErrorFormat("[ANDROID] WWW request on : {0} : {1} cloning of resources canceled", xml, www.error);
                        return;
                    }

                    data = www.downloadHandler.text;
                }

                string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                if (data.Length > 0 && data.StartsWith(_byteOrderMarkUtf8))
                {
                    data = data.Substring(_byteOrderMarkUtf8.Length);
                }
            }

            // Clone content in external directory with correct path
            var clone = new CloneManager();
            var doc = XDocument.Parse(data);

            var f = doc.Element("assets").Element("streamingAssets").Elements("file");

            foreach (var attribute in f.Attributes())
            {
                if (attribute.Name != "path") { continue; }

                //Update path for terminal
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
            Debug.Log(clone);
            await clone.Execute();
            Debug.LogWarningFormat("[JMH3] AndroidCloneResources() done");
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
                if (attribute.Name != "path") { continue; }
                attribute.SetValue(Application.dataPath.Replace("/base.apk", "/lib/arm64/"));
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
                    var path = pipeline.m_pipelinesPath[i];
                    if (path.Equals(data))
                    {
                        pipeline.m_configurationPath = path;
                        //pipeline.m_uuid = pipeline.m_pipelinesUUID[i];
                        pipeline.m_selectedPipeline = i;
                    }
                }
            }
        }

        class CloneManager
        {
            readonly List<Tuple<string, string>> m_data = new List<Tuple<string, string>>();

            public void Add(string source, string dest)
            {
                m_data.Add(Tuple.Create(source, dest));
            }

            async public Task Execute()
            {
                foreach (var tuple in m_data)
                {
                    await Clone(tuple.Item1, tuple.Item2);
                }
                m_data.Clear();
            }

            async Task Clone(string source, string dest)
            {
                //Create directory
                var path = Path.GetDirectoryName(dest);
                if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

                //WWW request
                byte[] data;
                using (var downloadHandler = new DownloadHandlerBuffer())
                using (var www = new UnityWebRequest(source) { downloadHandler = downloadHandler })
                {
                    var request = www.SendWebRequest();
                    while (!request.isDone) { await Task.Yield(); }

                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.LogErrorFormat("CloneFile error - {0} / {1}", source, www.error);
                        return;
                    }

                    data = www.downloadHandler.data;
                }

                if (File.Exists(dest)) File.Delete(dest);
                File.WriteAllBytes(dest, data);
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
}
