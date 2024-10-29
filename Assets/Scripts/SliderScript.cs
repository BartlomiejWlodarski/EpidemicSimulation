using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text attachedText;

    public void OnValueChanged()
    {
        attachedText.text = slider.value.ToString();
    }
}
