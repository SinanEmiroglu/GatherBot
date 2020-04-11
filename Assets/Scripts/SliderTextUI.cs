using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextUI : MonoBehaviour
{
    public Slider slider;

    TextMeshProUGUI textUI;

    void OnEnable()
    {
        textUI = GetComponent<TextMeshProUGUI>();
        slider.onValueChanged.AddListener(RefreshText);
    }

    void RefreshText(float value) => textUI.text = value.ToString();
    void OnDisable() => slider.onValueChanged.RemoveListener(RefreshText);
}