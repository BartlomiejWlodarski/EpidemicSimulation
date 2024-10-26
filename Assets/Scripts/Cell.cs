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
        diagram.n = population.N;
        diagram.s = (float)population.S / (float)population.N;
        diagram.e = (float)population.E / (float)population.N;
        diagram.i = (float)population.I / (float)population.N;
        diagram.r = (float)population.R / (float)population.N;
        diagram.d = (float)population.D / (float)population.N;

        diagram.UpdateDiagram();
    }
}
