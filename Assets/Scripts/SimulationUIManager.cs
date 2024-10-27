using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SimulationManager;

public class SimulationUIManager : MonoBehaviour
{
    public SimulationManager simulation;

    public List<TMP_Text> simulationInfoHeaders;

    private int lastDays = -1;


    private void Update()
    {
        if (simulation.days > lastDays)
        {
            lastDays = (int)simulation.days;
            UpdateSimulationInfoHeaders();
        }
    }

    private void UpdateSimulationInfoHeaders()
    {
        simulationInfoHeaders[0].text = simulation.days.ToString();
        simulationInfoHeaders[1].text = simulation.population.N.ToString();
        simulationInfoHeaders[2].text = simulation.population.S.ToString();
        simulationInfoHeaders[3].text = simulation.population.E.ToString();
        simulationInfoHeaders[4].text = simulation.population.I.ToString();
        simulationInfoHeaders[5].text = simulation.population.R.ToString();
        simulationInfoHeaders[6].text = simulation.population.D.ToString();
    }

}
