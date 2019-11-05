using System.IO;
using System.Text;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class TestPathAndroid : MonoBehaviour
{

    public string m_streamingAssetsFile;
    private string m_filePath;
    private string m_fileName;
    private string m_text = "";
 
    void Start()
    {
#if UNITY_ANDROID
#endif


        m_filePath = Application.streamingAssetsPath+ m_streamingAssetsFile;
        m_fileName = Path.GetFileName(m_streamingAssetsFile);

        string data = "";
        LoadWWW(m_filePath, ref data);
        LoadDemoXml(data);

        //Check if the directory already exist
        string pathAndroidSolAR = Application.persistentDataPath + "/AndroidSolAR/";
        if (!Directory.Exists(Path.GetDirectoryName(pathAndroidSolAR)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(pathAndroidSolAR));
        }

        Debug.Log("debug : " + pathAndroidSolAR + m_fileName);
        Debug.Log("debug : key ? - "+PlayerPrefs.HasKey("initAndroid"));
        //Check if file exist
        if (!File.Exists(pathAndroidSolAR + m_fileName))
        {
            CloneFile(m_filePath, pathAndroidSolAR + m_fileName);
            PlayerPrefs.SetInt("initAndroid", 0);  //Set First run
            Debug.Log("debug : ici 1");
        }
        else if (!PlayerPrefs.HasKey("initAndroid"))
        {
            CloneFile(m_filePath, pathAndroidSolAR + m_fileName);
            Debug.Log("debug : ici 2");
        }
        
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
    
    private void CloneFile(string source, string dest)
    {
        string output = "";
        LoadWWW(source, ref output);
        
        File.WriteAllText(dest, output);
    }

    private void LoadWWW(string filePath, ref string output)
    {
        WWW request = new WWW(filePath);

        while (!request.isDone)
        {/*Loading xml*/
        }

        if (string.IsNullOrEmpty(request.error))
        {
            output = request.text;
        }
        else
        {
            output = request.error;
        }
    }

    private void LoadDemoXml(string data)
    {
        string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        if (data.StartsWith(_byteOrderMarkUtf8))
        {
            data = data.Remove(0, _byteOrderMarkUtf8.Length);
        }

        var doc = XDocument.Parse(data.ToString());

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
