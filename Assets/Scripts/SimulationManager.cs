using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Cell;

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

    public float healthyCommuting = 0.055f; // phi_h
    public float infectedCommuting = 0.013f; // phi_s
    public float outsideCommuting = 0.23f; // phi_c

    public uint outsideCommutingTarget = 75000; // Minimum population for travelers commuting outside of neighborhood

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
        foreach (Cell cell in cells)
        {
            //CommutingSimulation((int)(cell.row * r + cell.col));
            
            cell.InfectionProbability(contactRate, variationCoefficient);

            // Infection()

            // CommutersInfection()

            // ReturnCommuters()

            // UpdateStates()

        }


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

    void CommutingSimulation(int index)
    {
        Cell cell = cells[index];

        uint healthyCommuters = (uint)(cell.population.S * healthyCommuting);
        uint healthyCommutersOutside = (uint)(healthyCommuters * outsideCommuting);
        uint healthyCommutersNeighborhood = healthyCommuters - healthyCommutersOutside;

        uint infectedCommuters = (uint)(cell.population.I * infectedCommuting);
        uint infectedCommutersOutside = (uint)(infectedCommuters * outsideCommuting);
        uint infectedCommutersNeighborhood = infectedCommuters - infectedCommutersOutside;


        // Possibly move the destinations initialization to start
        List<int> neighborhoodDestinations = new List<int>();

        // Finding the destinations in the neighborhood
        for (int i = -1; i <= 1 ; i++) 
        {
            for (int j = -1; j <= 1; j++)
            {
                int destRow = (int)cell.row + i;
                int destCol = (int)cell.col + j;

                if (destRow < 0 || destRow >= r ||  destCol < 0 || destCol >= c || (destRow == 0 && destCol == 0))
                {
                    continue;
                }

                if (cells[destRow * (int)r + destCol].population.N == 0)
                {
                    continue;
                }

                neighborhoodDestinations.Add(destRow * (int)r + destCol);
            }
        }


        // Finding the destination outside of the neighborhood
        int range = 2;
        int outsideDestination = -1;

        while (outsideDestination == -1)
        {
            int maxRow = (int)cell.row - range;
            int maxCol = (int)cell.col - range;

            for (int i = -range; i <= range; i++)
            {
                for (int j = -range; j <= range; j++)
                {
                    // Skipping iterations when the target is not on the edges
                    if (Math.Abs(i) != range && Math.Abs(j) != range)
                    {
                        continue;
                    }

                    int destRow = (int)cell.row + i;
                    int destCol = (int)cell.col + j;

                    if (destRow < 0 || destRow >= r || destCol < 0 || destCol >= c)
                    {
                        continue;
                    }

                    if (cells[destRow * (int)r + destCol].population.N == 0)
                    {
                        continue;
                    }

                    if (outsideDestination == -1)
                    {
                        maxRow = destRow;
                        maxCol = destCol;
                    }

                    if (cells[destRow * (int)r + destCol].population.N >= outsideCommutingTarget && cells[destRow * (int)r + destCol].population.N >= cells[maxRow * (int)r + maxCol].population.N)
                    {
                        maxRow = destRow;
                        maxCol = destCol;
                        outsideDestination = maxRow * (int)r + maxCol;
                    }
                }
            }

            range++;
        }

        // Finding commuters count for neighborhood destination
        int[] healthyCommutersDestinations = new int[neighborhoodDestinations.Count];

        for (int i = 0; i < healthyCommutersNeighborhood; i++)
        {
            int destination = UnityEngine.Random.Range(0, neighborhoodDestinations.Count);
            healthyCommutersDestinations[destination]++;
        }

        int[] infectedCommutersDestinations = new int[neighborhoodDestinations.Count];

        for (int i = 0; i < infectedCommutersNeighborhood; i++)
        {
            int destination = UnityEngine.Random.Range(0, neighborhoodDestinations.Count);
            infectedCommutersDestinations[destination]++;
        }

        // Neighborhood commuters
        for (int i = 0; i < neighborhoodDestinations.Count; i++)
        {
            cells[neighborhoodDestinations[i]].incomingTravelers.I += (uint)infectedCommutersDestinations[i];
            IncomingTravelers travelersNeighborhood;
            travelersNeighborhood.row = cell.row;
            travelersNeighborhood.col = cell.col;
            travelersNeighborhood.healthyTravelers = (uint)healthyCommutersDestinations[i];
            cells[neighborhoodDestinations[i]].healthyIncomingTravelers.Add(travelersNeighborhood);
        }

        // Outside travels
        cells[outsideDestination].incomingTravelers.I += infectedCommutersOutside;
        IncomingTravelers travelers;
        travelers.row = cell.row;
        travelers.col = cell.col;
        travelers.healthyTravelers = healthyCommutersOutside;
        cells[outsideDestination].healthyIncomingTravelers.Add(travelers);

        // Updating outgoing travelers
        cell.outgoingTravelers.I += infectedCommuters;
        cell.outgoingTravelers.S += healthyCommuters;
    }
}
