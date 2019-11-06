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
        CloneStreamingAssets(Application.streamingAssetsPath + "/SolAR/androidClone.xml", Application.persistentDataPath + "/Plugins/");
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
    
    private void CloneStreamingAssets(string pathToXml,string dest)
    {
        //Read XML.xml with HTTP request (due to .apk)
        WWW request = new WWW(pathToXml);
        while (!request.isDone)
        {/*Loading xml*/}
        if (!string.IsNullOrEmpty(request.error))
        {
            Debug.LogError("debug : error - " + pathToXml + " / " + request.error);
            return;
        }
 
        string data=request.text;
        string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        if (data.StartsWith(_byteOrderMarkUtf8))
        {
            data = data.Remove(0, _byteOrderMarkUtf8.Length);
        }

        //Clone all files to dest
        var doc = XDocument.Parse(data.ToString());
        var file = doc.Element("xpcf-registry").Elements("file");
        foreach (var attribute in file.Attributes())
        {
            CloneFile(Application.streamingAssetsPath + attribute.Value, dest + attribute.Value);
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
        if (!File.Exists(dest))
        {
            WWW request = new WWW(source);

            while (!request.isDone)
            {/*Loading xml*/}


            if (string.IsNullOrEmpty(request.error))
            {
                File.WriteAllBytes(dest, request.bytes);
            }
            else
            {
                Debug.LogError("debug : CloneFile error - "+source+" / " + request.error);
            }
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
    }
}
