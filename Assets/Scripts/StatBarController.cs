using UnityEngine;
using UnityEngine.UI;

public class StatBarController : MonoBehaviour
{
    public SimulationManager sim;
    public LayoutElement dead;
    public LayoutElement infected;
    public LayoutElement exposed;
    public LayoutElement suspectible; 
    public LayoutElement recovered;
    public RectTransform statBar;

    private SimulationManager.Population pop;

    public void UpdateBars()
    {
        pop = sim.population;
        float dRatio = (float)pop.D / pop.N;
        float iRatio = (float)pop.I / pop.N;
        float eRatio = (float)pop.E / pop.N;
        float sRatio = (float)pop.S / pop.N;
        float rRatio = (float)pop.R / pop.N;

        float maxWidth = statBar.sizeDelta.x;

        dead.preferredWidth = dRatio * maxWidth;
        infected.preferredWidth = iRatio * maxWidth;
        exposed.preferredWidth = eRatio * maxWidth;
        suspectible.preferredWidth = sRatio * maxWidth;
        recovered.preferredWidth = rRatio * maxWidth;
    }
}
