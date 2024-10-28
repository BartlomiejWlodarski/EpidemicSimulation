using UnityEngine;
using static SimulationManager;

public class Cell : MonoBehaviour
{
    public Population population;

    public CellDiagram diagram;

    public uint i;
    public uint j;

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
}
