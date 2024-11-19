using UnityEngine;

public class TravelerSize : MonoBehaviour
{
    private readonly float minSize = 0.0f;
    private readonly float maxSize = 0.13f;

    public void UpdateTravelerSize(uint N)
    {
        float destSize;

        // Hardcoded min and max values for amount of travelers incoming to cell
        N = (uint)Mathf.Clamp(N, 3000, 10000);
        destSize = Mathf.Lerp(minSize, maxSize, (N - 3000f) / (10000f - 3000f));
        this.transform.localScale = Vector3.one * destSize;
    }
}
