using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimulationManager : MonoBehaviour
{
    [System.Serializable]
    public struct Population
    {
        public uint N;
        public uint S;
        public uint E;
        public uint I;
        public uint R;
        public uint D;
    }

    public Population population;

    public List<Cell> cells; // Maybe change it to 2D array?

    public uint days = 0;

    public GameObject cellsGrid;

    public uint c;
    public uint r;


    [Header("Input data")]

    public uint exposedDuration = 2; // a
    public uint infectiveDuration = 4; // b

    public float variationCoefficient = 0.5f; // c_v

    public float contactRate = 0.5f; // beta

    public float mortalityRate; // u_vm
    public float deathsPerStep; // u_d
    public float birthsPerStep; // u_b

    public float healthyCommuting; // phi_h
    public float infectedCommuting; // phi_s
    public float outsideCommuting; // phi_c


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Add cells to the list
        for (int i = 1; i < cellsGrid.transform.childCount; i++)
        {
            cells.Add(cellsGrid.transform.GetChild(i).GetComponent<Cell>());
            cells[i - 1].col = (uint)(i - 1) % c;
            cells[i - 1].row = (uint)(i - 1 - cells[i - 1].col) / c;
        }
        
        // Initialize cells and diagrams
        foreach (Cell cell in cells)
        {
            cell.InitializeLists(exposedDuration, infectiveDuration);

            // If there are any infected/exposed, we are assuming they have been only one day in that state.
            cell.exposed[1] = cell.population.E;
            cell.infected[1] = cell.population.I;

            cell.UpdateDiagram();
        }

        // Update the total population
        UpdatePopulation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Step()
    {
        // Update cells and calculate
        Debug.Log("Cell(0, 0) infection probability: "+ cells[0].InfectionProbability(contactRate, variationCoefficient).ToString());

        // Update diagrams
        foreach (Cell cell in cells)
        {
            cell.UpdateDiagram();
        }

        // Update the total population
        UpdatePopulation();

        days++;
    }

    void UpdatePopulation()
    {
        Population p;
        p.N = 0;
        p.S = 0;
        p.E = 0;
        p.I = 0;
        p.R = 0;
        p.D = 0;

        foreach (Cell cell in cells)
        {
            p.N += cell.population.N;
            p.S += cell.population.S;
            p.E += cell.population.E;
            p.I += cell.population.I;
            p.R += cell.population.R;
            p.D += cell.population.D;
        }

        population = p;
    }
}
