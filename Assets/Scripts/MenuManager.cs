using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public List<SliderScript> sliders;

    public GameObject authorsView;
    public GameObject biblioView;
    public GameObject menuCanvas;
    public GameObject simCanvas;

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
}
