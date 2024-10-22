using UnityEngine;

public class CellDiagram : MonoBehaviour
{
    public float r = 0.1f;
    public float s = 0.2f;
    public float e = 0.3f;
    public float i = 0.3f;
    public float d = 0.1f;

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
        mR.GetPropertyBlock(mPB);

        mPB.SetFloat("_RecoveredPercent", r);
        mPB.SetFloat("_SusceptiblePercent", s);
        mPB.SetFloat("_ExposedPercent", e);
        mPB.SetFloat("_InfectedPercent", i);
        mPB.SetFloat("_DeadPercent", d);

        mR.SetPropertyBlock(mPB);
    }
}
