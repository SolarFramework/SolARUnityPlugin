using UnityEngine;

public class ExitKey : MonoBehaviour
{
    protected void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
