using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static SimulationManager;

public class Cell : MonoBehaviour
{
    // Healthy travelers are kept in sepearate list with their destination to calculate infection probabiliy correctly
    public struct IncomingTravelers
    {
        public uint row;
        public uint col;
        public uint healthyTravelers;
    }
    
    public Population population;

    // If needed change to list of travelers with specified state, origin/destination
    public Population incomingTravelers;
    public List<IncomingTravelers> healthyIncomingTravelers = new List<IncomingTravelers>(); 
    public Population outgoingTravelers;

    public float infectionProbability = 0.0f;

    public CellDiagram diagram;

    [Header("Indices")]
    public uint row;
    public uint col;

    [Header("States durations list")]
    // Lists storing how many people are in that state for a given number of days.
    // For example infected[2] denotes how many people have been infected for two days.  
    public List<uint> infected;
    public List<uint> exposed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateDiagram();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDiagram()
    {
        // For the sake of visualisation, we add the dead to the population.
        diagram.n = population.N + population.D;
        diagram.s = (float)population.S / (float)diagram.n;
        diagram.e = (float)population.E / (float)diagram.n;
        diagram.i = (float)population.I / (float)diagram.n;
        diagram.r = (float)population.R / (float)diagram.n;
        diagram.d = (float)population.D / (float)diagram.n;

        diagram.UpdateDiagram();
    }

    public void InitializeLists(uint a, uint b)
    {
        exposed = new List<uint>(new uint[a+1]);
        infected = new List<uint>(new uint[b+1]);
    }

    private float GaussianRandomNumber(float mean, float stdDev)
    {
        // Calculation using Box-Muller transform

        float u1 = Random.Range(0.0000001f, 1.0f);
        float u2 = Random.Range(0.0000001f, 1.0f);

        float z0 = Mathf.Sqrt(-2 * Mathf.Log10(u1)) * Mathf.Cos(2 * Mathf.PI * u2);
        //float z1 = Mathf.Sqrt(-2 * Mathf.Log10(u1)) * Mathf.Sin(2 * Mathf.PI * u2);

        return mean + stdDev * z0;
    }

    private float InfectionStrength(float contactRate, float variationCoefficient)
    {
        float mean = 0;
        float top = 0;

        for (int i = 1; i < infected.Count; i++)
        {
            top += infected[i];
        }

        top += incomingTravelers.I - outgoingTravelers.I;

        float bottom = population.N + incomingTravelers.N - outgoingTravelers.N;

        mean = 1 - Mathf.Exp(-contactRate * top / bottom);

        return GaussianRandomNumber(mean, variationCoefficient);
    }

    public void InfectionProbability(float contactRate, float variationCoefficient)
    {
        infectionProbability = Mathf.Clamp(InfectionStrength(contactRate, variationCoefficient), 0, 1);
    }
}
