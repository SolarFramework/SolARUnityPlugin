using System.IO;
using System.Text;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class TestPathAndroid : MonoBehaviour
{
    private string m_text = "";

    void Start()
    {
        CloneStreamingAssets("/SolAR/androidClone.xml", "/Plugins");
        Demo(Application.persistentDataPath + "/Plugins/SolAR/Pipelines/PipelineFiducialMarker.xml");
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
    
    private void CloneStreamingAssets(string xml,string dest)
    {
        //Use conf xml of the apk
        WWW request = new WWW(Application.streamingAssetsPath + xml);
        while (!request.isDone)
        {/*Loading xml*/}
        if (!string.IsNullOrEmpty(request.error))
        {
            Debug.LogError("debug : error - " + Application.streamingAssetsPath + xml + " / " + request.error);
            return;
        }

        string data = request.text;
        string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        if (data.StartsWith(_byteOrderMarkUtf8))
        {
            data = data.Remove(0, _byteOrderMarkUtf8.Length);
        }

        //Check if conf xml already exist on the terminal and should be used instead of the one of the apk
        if (File.Exists(Application.persistentDataPath + dest + xml))
        {
            StreamReader input = new StreamReader(Application.persistentDataPath + dest + xml);
            string tmp = input.ReadToEnd();
            var old = XDocument.Parse(tmp);
            var conf = old.Element("xpcf-registry").Elements("file");
            foreach (var attribute in conf.Attributes())
            {
                if (attribute.Name == "path" && attribute.Value==xml)
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
        var file = doc.Element("xpcf-registry").Elements("file");
        foreach (var attribute in file.Attributes())
        {
            if (attribute.Name == "path")
            {
                if(attribute.Parent.Attribute("overwrite") != null)
                {
                    if (attribute.Parent.Attribute("overwrite").Value.Equals("true"))
                    {
                        //Overwrite
                        CloneFile(Application.streamingAssetsPath + attribute.Value, Application.persistentDataPath+dest+"/"+ attribute.Value);
                    }
                }
                else
                {
                    //no information about overwrite, overwrite file
                    CloneFile(Application.streamingAssetsPath + attribute.Value, Application.persistentDataPath+dest+"/" + attribute.Value);
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
