using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;

public class TestPathAndroid : MonoBehaviour
{
    private string m_text = "";

    void Start()
    {        
            AndroidClone(Application.streamingAssetsPath+"/SolAR/androidClone.xml");
            Demo(Application.persistentDataPath + "/SolAR/Pipelines/PipelineFiducialMarker.xml");
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
        string fileName = Path.GetFileName(conf_xml);
        string dest = Path.GetDirectoryName(Regex.Split(conf_xml, Application.streamingAssetsPath)[1]);

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
        if (File.Exists(Application.persistentDataPath + dest + fileName))
        {
            StreamReader input = new StreamReader(Application.persistentDataPath + dest + fileName);
            string tmp = input.ReadToEnd();
            var old = XDocument.Parse(tmp);
            var conf = old.Element("xpcf-registry").Element("streamingAssets").Elements("file");
            foreach (var attribute in conf.Attributes())
            {
                if (attribute.Name == "path" && attribute.Value == fileName)
                {
                    if (attribute.Parent.Attribute("overwrite") != null)
                    {
                        if (attribute.Parent.Attribute("overwrite").Value.Equals("false"))
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
            doc.Element("xpcf-registry").Element("streamingAssets").Elements("file"),
            doc.Element("xpcf-registry").Element("plugins").Elements("file")
        };

        foreach(var f in file)
        {
            foreach (var attribute in f.Attributes())
            {
                if (attribute.Name == "path")
                {
                    if (attribute.Parent.Attribute("overwrite") != null)
                    {
                        if (attribute.Parent.Attribute("overwrite").Value.Equals("true"))
                        {
#if UNITY_ANDROID && !UNITY_EDITOR
                            //Overwrite
                            CloneFile(Application.streamingAssetsPath + attribute.Value, Application.persistentDataPath + "/" + attribute.Value);
#elif UNITY_EDITOR
                            //Overwrite
                            if (attribute.Value.Contains("Plugins")) {
                                CloneFile(Path.GetDirectoryName(Application.streamingAssetsPath) + attribute.Value, Application.persistentDataPath + "/" + attribute.Value);
                            } else
                            {
                                CloneFile(Application.streamingAssetsPath + attribute.Value, Application.persistentDataPath + "/" + attribute.Value);
                            }
#endif
                        }
                    }
                    else
                    {
                        //no information about overwrite, overwrite file
                        CloneFile(Application.streamingAssetsPath + attribute.Value, Application.persistentDataPath + "/" + attribute.Value);
                    }

                }
            }
        }

    }
    private void CloneFile(string source, string dest)
    {
        //Create directory
        if (!Directory.Exists(Path.GetDirectoryName(dest)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dest));
        }

        //Create file
        WWW request = new WWW(source);
        while (!request.isDone)
        {/*Loading xml*/}

        if (string.IsNullOrEmpty(request.error))
        {
            File.Delete(dest);
            File.WriteAllBytes(dest, request.bytes);
        }
        else
        {
            Debug.LogError("debug : CloneFile error - "+source+" / " + request.error);
        }
        
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
}
