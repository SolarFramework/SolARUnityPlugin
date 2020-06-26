using SolAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AndroidMenu : MonoBehaviour
{
    public SolARPipeline m_solarPipeline;
    public GameObject m_AndroidMenu;
    public Button m_AndroidButton;
    public Dropdown m_pipelineDropdown;
    public GameObject m_AndroidTitle;
    public GameObject m_AndroidPopup;

    private void Awake()
    {
    #if !UNITY_ANDROID
        //Disable gameObject if platform is not Android. As only one pipeline configuration is edited in build process for other platforms 
         this.gameObject.SetActive(false);   
    #endif
    }

    void Start()
    {
        //AndroidButton
        m_AndroidButton.onClick.AddListener(delegate { Load(); });
        m_AndroidTitle.GetComponentInChildren<Text>().text = Application.productName+" - v"+Application.version;
        //Pipeline
        m_pipelineDropdown.ClearOptions();
        m_pipelineDropdown.AddOptions(new List<string>(m_solarPipeline.m_pipelinesName));
    }

    /**
     * <summary>
     *  Open or close the UI depending on gameobject's state
     * </summary>
     */
    private void Load()
    {
        //Enable or disable AndroidMenu canvas
        m_AndroidMenu.SetActive(!m_AndroidMenu.activeInHierarchy);
        EventSystem.current.SetSelectedGameObject(null);

        if (!m_AndroidMenu.activeInHierarchy) Close();
        else Open();
    }

    /**
     * <summary>
     * Open UI and select pipeline used
     * </summary>
     * */
    private void Open()
    {
       foreach(ConfXml.Module module in m_solarPipeline.conf.modules)
        {
            foreach(ConfXml.Module.Component component in module.components)
            {
                //Debug.Log(module.name + " - " + component.name);
            }

        }
        m_AndroidTitle.SetActive(true);
        m_pipelineDropdown.value = m_solarPipeline.m_selectedPipeline;
    }

    /**
     * <summary>
     * Close UI and process to change 
     * </summary>
     * */
    private void Close()
    {
        m_AndroidTitle.SetActive(false);
        //On close check if pipeline and camera need to be reload
        if (m_solarPipeline.m_selectedPipeline != m_pipelineDropdown.value)
        {
            m_solarPipeline.m_pipelineManager.stop();
            m_solarPipeline.m_webCamTexture.Stop();
            m_solarPipeline.m_pipelineManager.Dispose();
            m_solarPipeline.m_selectedPipeline = m_pipelineDropdown.value;
            m_solarPipeline.m_configurationPath = m_solarPipeline.m_pipelinesPath[m_solarPipeline.m_selectedPipeline];
            m_solarPipeline.m_uuid = m_solarPipeline.m_pipelinesUUID[m_solarPipeline.m_selectedPipeline];
            Android.SaveConfiguration(m_solarPipeline.m_configurationPath);
            m_solarPipeline.Init();
            StartCoroutine(Fade(m_AndroidPopup.GetComponent<Image>(),m_AndroidPopup.GetComponentInChildren<Text>()));
        }
    }

    private IEnumerator Fade(Image img, Text text)
    {
        img.gameObject.SetActive(true);
        for (float ft = 1f; ft >= 0; ft -= 0.01f)
        {
            Color c = img.color;
            c.a = ft;
            img.color = c;

            Color c2 = text.color;
            c2.a = ft;
            text.color = c2;
            yield return new WaitForSeconds(.02f);
        }
        img.gameObject.SetActive(false);
    }
}
