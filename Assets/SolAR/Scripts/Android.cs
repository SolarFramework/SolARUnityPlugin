using System.IO;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Android
{
    public static void AndroidCloneResources(string xml)
    {
        string data;
        string old_xml = Application.persistentDataPath + xml.Replace(Application.streamingAssetsPath, "/StreamingAssets");

        // Check if conf xml already exist in internal storage
        if (File.Exists(old_xml))
        {
            xml = old_xml;
            Debug.LogWarning("[ANDROID] Configuration xml found in internal memory : " + xml);
        }
        else
        {
            Debug.LogWarning("[ANDROID] No configuration xml found in internal memory : " + old_xml);
        }
        Debug.Log("[ANDROID] " + xml + " is used for configuration");

        // Get configuration xml data
        if (!xml.Equals(old_xml))
        {
            //WWW request on .apk 
            WWW request = new WWW(xml);
            while (!request.isDone) { }
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError("[ANDROID] WWW request on : " + xml + " : " + request.error);
                return;
            }

            data = request.text;
            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (data.StartsWith(_byteOrderMarkUtf8))
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
        CloneManager CloneManager = new CloneManager();
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
                        CloneManager.Add(src, output);
                    }
                }
            }
        }
        Debug.Log(CloneManager);
        CloneManager.Execute();
    }
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
                if (attribute.Value.Contains("Plugins"))
                {
                    attribute.SetValue(Application.dataPath.Replace("/base.apk", "/lib/arm64/"));
                }
            }
        }

        input.Close();
        doc.Save(filepath);
        return;
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

            //Create file
            WWW request = new WWW(source);
            while (!request.isDone)
            {
                //Loading xml
            }

            if (string.IsNullOrEmpty(request.error))
            {
                if (File.Exists(dest)) File.Delete(dest);
                File.WriteAllBytes(dest, request.bytes);
            }
            else
            {
                Debug.LogError("CloneFile error - " + source + " / " + request.error);
            }
            request.Dispose();
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
