using UnityEngine;

public class CellDiagram : MonoBehaviour
{
    public float r = 0.1f;
    public float s = 0.2f;
    public float e = 0.3f;
    public float i = 0.3f;
    public float d = 0.1f;

    public Material material;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_RecoveredPercent", r);
        material.SetFloat("_SusceptiblePercent", s);
        material.SetFloat("_ExposedPercent", e);
        material.SetFloat("_InfectedPercent", i);
        material.SetFloat("_DeadPercent", d);
    }
}
