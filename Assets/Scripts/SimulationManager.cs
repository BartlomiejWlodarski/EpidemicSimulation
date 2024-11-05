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
            CommutingSimulation(cell);
        }

        foreach (Cell cell in cells)
        {
            cell.InfectionProbability(contactRate, variationCoefficient);

            Infection(cell);
        }

        foreach (Cell cell in cells)
        {
            CommutersInfection(cell);
            ReturnCommuters(cell);
        }

        //foreach (Cell cell in cells)
        {
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

    void CommutingSimulation(Cell cell)
    {
        uint healthyCommuters = (uint)((cell.population.S + cell.population.R) * healthyCommuting);
        uint healthyCommutersOutside = (uint)(healthyCommuters * outsideCommuting);
        uint healthyCommutersNeighborhood = healthyCommuters - healthyCommutersOutside;

        uint nonHealthyCommuters = (uint)((cell.population.I + cell.population.E) * infectedCommuting);
        uint nonHealthyCommutersOutside = (uint)(nonHealthyCommuters * outsideCommuting);
        uint nonHealthyCommutersNeighborhood = nonHealthyCommuters - nonHealthyCommutersOutside;

        uint recoveredCommutersOutside = 0;
        uint recoveredCommutersNeighborhood = 0;

        uint susceptibleCommutersOutside = 0;
        uint susceptibleCommutersNeighborhood = 0;

        uint infectedCommutersOutside = 0;
        uint infectedCommutersNeighborhood = 0;

        uint exposedCommutersOutside = 0;
        uint exposedCommutersNeighborhood = 0;

        // Calculating commuters for each type and state

        for (int i = 0; i < healthyCommutersOutside; i++)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) < (float)cell.population.S / (float)(cell.population.S + cell.population.R))
            {
                susceptibleCommutersOutside++;
            }
        }

        recoveredCommutersOutside = healthyCommutersOutside - susceptibleCommutersOutside;

        for (int i = 0; i < healthyCommutersNeighborhood; i++)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) < (float)cell.population.S / (float)(cell.population.S + cell.population.R))
            {
                susceptibleCommutersNeighborhood++;
            }
        }

        recoveredCommutersNeighborhood = healthyCommutersNeighborhood - susceptibleCommutersNeighborhood;

        for (int i = 0; i < nonHealthyCommutersOutside; i++)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) < (float)cell.population.I / (float)(cell.population.I + cell.population.E))
            {
                infectedCommutersOutside++;
            }
        }

        exposedCommutersOutside = nonHealthyCommutersOutside - infectedCommutersOutside;

        for (int i = 0; i < nonHealthyCommutersNeighborhood; i++)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) < (float)cell.population.I / (float)(cell.population.I + cell.population.E))
            {
                infectedCommutersNeighborhood++;
            }
        }

        exposedCommutersNeighborhood = nonHealthyCommutersNeighborhood - infectedCommutersNeighborhood;

        // Possibly move the destinations initialization to start
        List<int> neighborhoodDestinations = new List<int>();

        // Finding the destinations in the neighborhood
        for (int i = -1; i <= 1 ; i++) 
        {
            for (int j = -1; j <= 1; j++)
            {
                int destRow = (int)cell.row + i;
                int destCol = (int)cell.col + j;

                if (destRow < 0 || destRow >= r ||  destCol < 0 || destCol >= c || (destRow == cell.row && destCol == cell.col))
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
        Population[] commutersDestinations = new Population[neighborhoodDestinations.Count];

        for (int i = 0; i < susceptibleCommutersNeighborhood; i++)
        {
            int destination = UnityEngine.Random.Range(0, neighborhoodDestinations.Count);
            commutersDestinations[destination].S++;
        }

        for (int i = 0; i < recoveredCommutersNeighborhood; i++)
        {
            int destination = UnityEngine.Random.Range(0, neighborhoodDestinations.Count);
            commutersDestinations[destination].R++;
        }

        for (int i = 0; i < infectedCommutersNeighborhood; i++)
        {
            int destination = UnityEngine.Random.Range(0, neighborhoodDestinations.Count);
            commutersDestinations[destination].I++;
        }

        for (int i = 0; i < exposedCommutersNeighborhood; i++)
        {
            int destination = UnityEngine.Random.Range(0, neighborhoodDestinations.Count);
            commutersDestinations[destination].E++;
        }

        // Neighborhood commuters
        for (int i = 0; i < neighborhoodDestinations.Count; i++)
        {
            cells[neighborhoodDestinations[i]].incomingTravelers.I += (uint)commutersDestinations[i].I;
            cells[neighborhoodDestinations[i]].incomingTravelers.E += (uint)commutersDestinations[i].E;
            cells[neighborhoodDestinations[i]].incomingTravelers.R += (uint)commutersDestinations[i].R;

            IncomingTravelers travelersNeighborhood;
            travelersNeighborhood.row = cell.row;
            travelersNeighborhood.col = cell.col;
            travelersNeighborhood.susceptibleTravelers = (uint)commutersDestinations[i].S;
            cells[neighborhoodDestinations[i]].healthyIncomingTravelers.Add(travelersNeighborhood);
        }

        // Outside commuters
        cells[outsideDestination].incomingTravelers.I += infectedCommutersOutside;
        cells[outsideDestination].incomingTravelers.E += exposedCommutersOutside;
        cells[outsideDestination].incomingTravelers.R += recoveredCommutersOutside;

        IncomingTravelers travelers;
        travelers.row = cell.row;
        travelers.col = cell.col;
        travelers.susceptibleTravelers = susceptibleCommutersOutside;
        cells[outsideDestination].healthyIncomingTravelers.Add(travelers);

        // Updating outgoing travelers
        cell.outgoingTravelers.I += infectedCommutersNeighborhood + infectedCommutersOutside;
        cell.outgoingTravelers.S += susceptibleCommutersNeighborhood + susceptibleCommutersOutside;
        cell.outgoingTravelers.E += exposedCommutersNeighborhood + exposedCommutersOutside;
        cell.outgoingTravelers.R += recoveredCommutersNeighborhood + recoveredCommutersOutside;
    }

    void ReturnCommuters(Cell cell)
    {
        cell.outgoingTravelers.S = 0;
        cell.outgoingTravelers.R = 0;
        cell.outgoingTravelers.I = 0;
        cell.outgoingTravelers.E = 0;
        cell.outgoingTravelers.N = 0;
        
        cell.incomingTravelers.R = 0;
        cell.incomingTravelers.I = 0;
        cell.incomingTravelers.E = 0;
        cell.incomingTravelers.N = 0;

        // TODO: Add infect travelers

        cell.healthyIncomingTravelers.Clear();
    }

    void Infection(Cell cell) 
    {
        for (int i = 0; i < cell.population.S - cell.outgoingTravelers.S; i++)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= cell.infectionProbability)
            {
                cell.population.S--;
                cell.population.E++;
                cell.exposed[0]++;
            }
        }
    }

    void CommutersInfection(Cell cell)
    {
        for (int i = 0; i < cell.healthyIncomingTravelers.Count; i++)
        {
            Cell destination = cells[(int)(cell.healthyIncomingTravelers[i].row * r + cell.healthyIncomingTravelers[i].col)];
            for (int j = 0; j < cell.healthyIncomingTravelers[i].susceptibleTravelers; j++)
            {
                if (UnityEngine.Random.Range(0.0f, 1.0f) <= cell.infectionProbability)
                {
                    destination.population.S--;
                    destination.population.E++;
                    destination.exposed[0]++;
                }
            }
        }
    }
}
