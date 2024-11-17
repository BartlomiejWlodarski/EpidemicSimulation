using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private SimulationManager sim;

    public List<SliderScript> sliders;
    public List<Button> mapSelectionButtons;

    public GameObject authorsView;
    public GameObject biblioView;
    public GameObject menuCanvas;
    public GameObject simCanvas;

    Color selectedMapcolor = new Color(0, 214.0f / 255.0f, 214.0f / 255.0f);
    Color notSelectedMapColor = new Color(5.0f / 255.0f, 196.0f / 255.0f, 107.0f / 255.0f);

    private void Start()
    {
        sim = FindFirstObjectByType<SimulationManager>().GetComponent<SimulationManager>();
        if (sim == null)
        {
            Debug.LogError("[NULL REFERENCE EXCEPTION]: Did not found Simulation Manager");
        }
    }

    public void OnStartClick()
    {
        simCanvas.SetActive(true);
        menuCanvas.SetActive(false);
    }

    public void OnResetParametersClick()
    {
        foreach (var slider in sliders)
        {
            slider.ResetToDefault();
        }

        OnSelectMap(0);
    }

    public void OnBiblioClick()
    {
        biblioView.SetActive(!biblioView.activeSelf);
    }

    public void OnAuthorsClick()
    {
        authorsView.SetActive(!authorsView.activeSelf);
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void CloseView(GameObject target)
    {
        target.SetActive(false);
    }

    public void OnSelectMap(int mapIndex)
    {
        for (int i = 0; i < mapSelectionButtons.Count; i++)
        {
            if (i == mapIndex)
            {
                mapSelectionButtons[i].GetComponent<Image>().color = selectedMapcolor;
            }
            else
            {
                mapSelectionButtons[i].GetComponent<Image>().color = notSelectedMapColor;
            }
        }
        sim.mapID = (uint)mapIndex;
    }
}
