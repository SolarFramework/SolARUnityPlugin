using SolAR;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public SolARPipeline m_solarPipeline;
    public GameObject m_optionsMenu;
    public Button m_optionsButton;
    public Dropdown m_pipelineDropdown;

    void Start() {
        m_optionsButton.onClick.AddListener(delegate { m_optionsMenu.SetActive(!m_optionsMenu.activeInHierarchy); EventSystem.current.SetSelectedGameObject(null); });

        List<string> pipelines = new List<string>();
        for (int i = 0; i < m_solarPipeline.m_pipelinesName.Length; i++) pipelines.Add(m_solarPipeline.m_pipelinesName[i]);
        m_pipelineDropdown.ClearOptions();
        m_pipelineDropdown.AddOptions(pipelines);
        m_pipelineDropdown.value = m_solarPipeline.m_selectedPipeline;
        m_pipelineDropdown.onValueChanged.AddListener(delegate{ m_solarPipeline.ChangePipeline(); });
    }	
}
