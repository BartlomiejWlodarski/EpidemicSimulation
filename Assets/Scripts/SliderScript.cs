using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SliderScript : MonoBehaviour
{
    enum Parameter
    { 
        a,
        b,
        c_v,
        beta,
        u_vm,
        u_d,
        u_b,
        phi_h,
        phi_s,
        phi_c
    }

    struct DefaultParameters
    {
        public uint a;
        public uint b;
        public float c_v;
        public float beta;
        public float u_vm;
        public float u_d;
        public float u_b;
        public float phi_h;
        public float phi_s;
        public float phi_c;
    }

    [SerializeField] Slider slider;
    [SerializeField] TMP_Text attachedText;
    [SerializeField] Parameter par;

    private SimulationManager sim;

    private readonly Dictionary<Parameter, float> defaultValues = new Dictionary<Parameter, float>();

    private void Awake()
    {
        // Define default values for each parameter
        defaultValues[Parameter.a] = 2;
        defaultValues[Parameter.b] = 4;
        defaultValues[Parameter.c_v] = 0.05f;
        defaultValues[Parameter.beta] = 0.5f;
        defaultValues[Parameter.u_vm] = 0.026f;
        defaultValues[Parameter.u_d] = 0.0000270f;
        defaultValues[Parameter.u_b] = 0.0000277f;
        defaultValues[Parameter.phi_h] = 0.055f;
        defaultValues[Parameter.phi_s] = 0.013f;
        defaultValues[Parameter.phi_c] = 0.23f;
    }

    private void Start()
    {
        sim = FindFirstObjectByType<SimulationManager>().GetComponent<SimulationManager>();
        if(sim == null)
        {
            Debug.LogError("[NULL REFERENCE EXCEPTION]: Did not found Simulation Manager");
        }
        ResetToDefault();
    }


    public void OnValueChanged()
    {
        UpdateTextBox();
        UpdateSimulationParameter();
    }

    private void UpdateSimulationParameter()
    {
        // Use the selected parameter to update the corresponding property in SimulationManager
        switch (par)
        {
            case Parameter.a:
                sim.exposedDuration = (uint)slider.value; break;
            case Parameter.b:
                sim.infectiveDuration = (uint)slider.value; break;
            case Parameter.c_v:
                sim.variationCoefficient = slider.value; break;
            case Parameter.beta:
                sim.contactRate = slider.value; break;
            case Parameter.u_vm:
                sim.mortalityRate = slider.value; break;
            case Parameter.u_d:
                sim.deathsPerStep = slider.value; break;
            case Parameter.u_b:
                sim.birthsPerStep = slider.value; break;
            case Parameter.phi_h:
                sim.healthyCommuting = slider.value; break;
            case Parameter.phi_s:
                sim.infectedCommuting = slider.value; break;
            case Parameter.phi_c:
                sim.outsideCommuting = slider.value; break;
            default:
                Debug.LogError("[INVALID PARAMETER EXCEPTION]: Incorrect Parameter");
                break;
        }
    }

    public void ResetToDefault()
    {
        // Set slider value to default based on the selected parameter
        if (defaultValues.TryGetValue(par, out float defaultValue))
        {
            slider.value = defaultValue;
        }
        else
        {
            Debug.LogError("[DEFAULT VALUE ERROR]: Default value for parameter not found");
        }
        OnValueChanged();
    }
    private void UpdateTextBox()
    {
        attachedText.text = slider.value.ToString();
    }
}
