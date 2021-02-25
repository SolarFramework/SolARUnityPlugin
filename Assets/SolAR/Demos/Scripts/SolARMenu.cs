using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SolAR
{
    public class SolARMenu : MonoBehaviour
    {
        [UnityEngine.Serialization.FormerlySerializedAs("m_solarPipeline")]
        public SolARPipeline solarPipeline;
        [UnityEngine.Serialization.FormerlySerializedAs("m_menu")]
        public GameObject menuGO;
        [UnityEngine.Serialization.FormerlySerializedAs("m_button")]
        public Button loadButton;
        [UnityEngine.Serialization.FormerlySerializedAs("m_pipelineDropdown")]
        public Dropdown pipelinesDropdown;
        public GameObject m_title;
        public GameObject m_popup;

        protected void Start()
        {
            //Button
            loadButton.onClick.AddListener(Load);
            m_title.GetComponentInChildren<Text>().text = Application.productName + " - v" + Application.version;
            //Pipeline
            pipelinesDropdown.ClearOptions();
            pipelinesDropdown.AddOptions(new List<string>(solarPipeline.m_pipelinesName));
        }

        /**
         * <summary>
         *  Open or close the UI depending on gameobject's state
         * </summary>
         */
        void Load()
        {
            //Enable or disable SolARMenu canvas
            menuGO.SetActive(!menuGO.activeInHierarchy);
            EventSystem.current.SetSelectedGameObject(null);

            if (menuGO.activeInHierarchy)
                Open();
            else
                Close();
        }

        /**
         * <summary>
         * Open UI and select pipeline used
         * </summary>
         * */
        void Open()
        {
            ///*
            foreach (var module in solarPipeline.conf.modules)
            {
                foreach (var component in module.components)
                {
                    Debug.LogFormat(this, "{0} - {1}", module.name, component.name);
                }
            }
            // */
            m_title.SetActive(true);
            pipelinesDropdown.value = solarPipeline.m_selectedPipeline;
        }

        /**
         * <summary>
         * Close UI and process to change 
         * </summary>
         * */
        void Close()
        {
            m_title.SetActive(false);
            //On close check if pipeline and camera need to be reload
            if (solarPipeline.m_selectedPipeline != pipelinesDropdown.value)
            {
                solarPipeline.pipelineManager.stop();
                solarPipeline.webcamTexture.Stop();
                solarPipeline.pipelineManager.Dispose();
                solarPipeline.m_selectedPipeline = pipelinesDropdown.value;
                solarPipeline.m_configurationPath = solarPipeline.m_pipelinesPath[solarPipeline.m_selectedPipeline];
                //solarPipeline.m_uuid = solarPipeline.m_pipelinesUUID[solarPipeline.m_selectedPipeline];
                Android.SaveConfiguration(solarPipeline.m_configurationPath);
                solarPipeline.Init();
                StartCoroutine(FadeOut(m_popup.GetComponent<Image>(), m_popup.GetComponentInChildren<Text>()));
            }
        }

        IEnumerator FadeOut(Image img, Text text)
        {
            img.gameObject.SetActive(true);
            for (float ft = 1f; ft >= 0; ft -= Time.deltaTime / 2)
            {
                Color c1 = img.color;
                c1.a = ft;
                img.color = c1;

                Color c2 = text.color;
                c2.a = ft;
                text.color = c2;

                yield return null;
            }
            img.gameObject.SetActive(false);
        }
    }
}
