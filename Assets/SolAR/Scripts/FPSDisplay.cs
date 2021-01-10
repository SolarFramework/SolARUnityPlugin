using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;

    protected void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    protected void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        var rect = new Rect(0, 0, w, h * 2 / 100);
        var style = new GUIStyle
        {
            alignment = TextAnchor.UpperLeft,
            fontSize = h * 2 / 100,
        };
        style.normal.textColor = new Color(0.0f, 1f, 1f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
