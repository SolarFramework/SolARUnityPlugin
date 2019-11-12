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
    
    private void AndroidClone(string conf_xml)
    {
        CloneManager CloneManager = new CloneManager();
        //Use conf xml of the apk
        WWW request = new WWW(conf_xml);
        while (!request.isDone)
        {/*Loading xml*/}
        if (!string.IsNullOrEmpty(request.error))
        {
            Debug.LogError("debug : error - " + conf_xml + " / " + request.error);
            return;
        }

        string data = request.text;
        string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        if (data.StartsWith(_byteOrderMarkUtf8))
        {
            data = data.Remove(0, _byteOrderMarkUtf8.Length);
        }

        //Check if conf xml already exist on the terminal and should be used instead of the one of the apk
        string old_conf_xml = Path.GetFullPath(conf_xml).Replace(Path.GetDirectoryName(Application.streamingAssetsPath), Application.persistentDataPath);
        if (File.Exists(old_conf_xml))
        {
            StreamReader input = new StreamReader(old_conf_xml);
            string tmp = input.ReadToEnd();
            var old = XDocument.Parse(tmp);
            var conf = old.Element("assets").Element("streamingAssets").Elements("file");
            foreach (var attribute in conf.Attributes())
            {
                if (attribute.Name == "path" && attribute.Value.Contains(Path.GetFileName(conf_xml)))
                {
                    if (attribute.Parent.Attribute("overWrite") != null)
                    {
                        if (attribute.Parent.Attribute("overWrite").Value.Equals("false"))
                        {
                            //keep terminal xml
                            data = tmp;
                        }
                    }
                }
            }
            input.Close();
        }

        //Clone from conf xml selected
        var doc = XDocument.Parse(data.ToString());
        var file = new[] {
            doc.Element("assets").Element("streamingAssets").Elements("file"),
            doc.Element("assets").Element("plugins").Elements("file")
        };

        foreach(var f in file)
        {
            foreach (var attribute in f.Attributes())
            {
                if (attribute.Name == "path")
                {
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

                    if (!File.Exists(output))
                    {
                        //Overwrite
                        CloneManager.Add(src, output);
                    }
                    else
                    {
                        if (attribute.Parent.Attribute("overWrite") != null)
                        {
                            if (attribute.Parent.Attribute("overWrite").Value.Equals("true"))
                            {
                                //Overwrite
                                CloneManager.Add(src, output);
                            }
                        }
                        else
                        {
                            //no information about overwrite, overwrite file
                            CloneManager.Add(src, output);
                        }
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
