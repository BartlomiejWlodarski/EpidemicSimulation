using UnityEngine;
using System;
using Unity.Mathematics;

public class CellDiagram : MonoBehaviour
{
    public uint n = 100000;
    public float r = 0.1f;
    public float s = 0.2f;
    public float e = 0.3f;
    public float i = 0.3f;
    public float d = 0.1f;

    public float baseHeight = 0.8f; // Model height in scale = 1
    public float h = 1f;

    public const float minSize = 0.5f;
    public const float maxSize = 1.6f;
    public const int minPopulation = 100;
    public const int maxPopulation = 100000;

    public Material material;
    [SerializeField] protected MeshRenderer mR;
    protected MaterialPropertyBlock mPB;

    private void Awake()
    {
        mPB = new MaterialPropertyBlock();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDiagram()
    {
        float populationSize = Math.Clamp(n, minPopulation, maxPopulation);
        h = (((populationSize - minPopulation) / (maxPopulation - minPopulation)) * (maxSize - minSize)) + minSize;
        GetComponent<Transform>().localScale = new Vector3(h, h, h);
        h *= baseHeight;

        mR.GetPropertyBlock(mPB);

        mPB.SetFloat("_RecoveredPercent", r);
        mPB.SetFloat("_SusceptiblePercent", s);
        mPB.SetFloat("_ExposedPercent", e);
        mPB.SetFloat("_InfectedPercent", i);
        mPB.SetFloat("_DeadPercent", d);
        mPB.SetFloat("_Height", h);

        mR.SetPropertyBlock(mPB);
    }
}
