using System.IO;
using System.Text;
using System.Xml.Linq;
using Unity.Collections;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestPathAndroid : MonoBehaviour
{
    private string m_text = "";

    void Start()
    {
        Debug.Log("path : " + Application.persistentDataPath);
        AndroidClone(Application.streamingAssetsPath+"/SolAR/Android/android.xml");
        Demo(Application.persistentDataPath + "/StreamingAssets/SolAR/Pipelines/PipelineFiducialMarker.xml");
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 1f, 1f, 1.0f);
        GUI.Label(rect, m_text, style);
    }
    
    private void AndroidClone(string androidXml)
    {
        string data = "";

        //1.Check if android.xml should be overwrite or use
        string old_xml = Path.GetFullPath(androidXml).Replace(Path.GetDirectoryName(Application.streamingAssetsPath), Application.persistentDataPath).Replace("\\","/");

        if (File.Exists(old_xml))
        {
            StreamReader input = new StreamReader(old_xml);
            string tmp = input.ReadToEnd();
            var old = XDocument.Parse(tmp);
            var conf = old.Element("assets").Element("streamingAssets").Elements("file");
            foreach (var attribute in conf.Attributes())
            {
                if (attribute.Name == "path" && attribute.Value.Contains(Path.GetFileName(old_xml)))
                {
                    if (attribute.Parent.Attribute("overWrite") != null && attribute.Parent.Attribute("overWrite").Value.Equals("false"))
                    {
                        //keep the xml already on terminal
                        data = tmp;
                    }
                }
            }
            input.Close();
        }
        //2.Get android.xml from .apk archive if local android.xml isn't used
        if (string.IsNullOrEmpty(data))
        {
            WWW request = new WWW(androidXml);
            while (!request.isDone) { }
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError("debug : error - " + androidXml + " / " + request.error);
                return;
            }

            data = request.text;
            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (data.StartsWith(_byteOrderMarkUtf8))
            {
                data = data.Remove(0, _byteOrderMarkUtf8.Length);
            }
        }

        //3. Clone content in external directory with correct path
        CloneManager CloneManager = new CloneManager();
        var doc = XDocument.Parse(data.ToString());
        var file = new[] {
            doc.Element("assets").Element("streamingAssets").Elements("file"),
            doc.Element("assets").Element("plugins").Elements("file")
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
                        src = attribute.Value.Replace("./Assets/StreamingAssets", Application.streamingAssetsPath);
                        output = attribute.Value.Replace("./Assets", Application.persistentDataPath);
                    }
                    else
                    {
#if UNITY_EDITOR
                        src = attribute.Value.Replace("./Assets", Application.dataPath);   // Plugins are in ./Assets/Plugins in editor
                        output = attribute.Value.Replace("./Assets", Application.persistentDataPath);
#elif UNITY_ANDROID
                        src = attribute.Value.Replace("./Assets", Application.streamingAssetsPath); // Plugins should be extracted from AndroidStreamingAssetsPath which (ie :  [apk]/assets/Plugins), no the same path than dataPath
                        output = attribute.Value.Replace("./Assets", Application.persistentDataPath);
#endif
                    }

                    if(attribute.Parent.Attribute("overWrite")==null || attribute.Parent.Attribute("overWrite").Equals("true") || !File.Exists(output)){
                        //Overwrite
                        CloneManager.Add(src, output);
                    }
                }
            }
        }
        CloneManager.Execute();
    }

    private void Demo(string filepath)
    {
        StreamReader input = new StreamReader(filepath);
        var doc = XDocument.Parse(input.ReadToEnd());
        var module = doc.Element("xpcf-registry").Elements("module");
        foreach (var attribute in module.Attributes())
        {
            if (attribute.Name == "name")
            {
                m_text += attribute.Value + "\n";
            }
        }
        input.Close();
    }

    private class CloneManager
    {
        private List<Tuple<string, string>> m_data;
        public CloneManager()
        {
            m_data = new List<Tuple<string, string>>();
        }

        public void Add(string source,string dest)
        {
            m_data.Add(new Tuple<string, string>(source, dest));
        }

        public void Execute()
        {
            foreach(Tuple<string,string> c in m_data)
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
                Debug.LogError("debug : CloneFile error - " + source + " / " + request.error);
            }
            request.Dispose();
        }
    }

    
}
