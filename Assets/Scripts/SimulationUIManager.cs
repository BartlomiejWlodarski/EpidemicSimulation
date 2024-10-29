using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimulationUIManager : MonoBehaviour
{
    public SimulationManager sim;

    public List<TMP_Text> simulationInfoHeaders;
    public StatBarController statBar;

    private int lastDays = -1;


    private void Update()
    {
        if (sim.days > lastDays)
        {
            lastDays = (int)sim.days;
            UpdateSimulationInfoHeaders();
        }
    }

    private void UpdateSimulationInfoHeaders()
    {
        simulationInfoHeaders[0].text = sim.days.ToString();
        simulationInfoHeaders[1].text = sim.population.N.ToString();
        simulationInfoHeaders[2].text = sim.population.S.ToString();
        simulationInfoHeaders[3].text = sim.population.E.ToString();
        simulationInfoHeaders[4].text = sim.population.I.ToString();
        simulationInfoHeaders[5].text = sim.population.R.ToString();
        simulationInfoHeaders[6].text = sim.population.D.ToString();
   
        statBar.UpdateBars();
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
