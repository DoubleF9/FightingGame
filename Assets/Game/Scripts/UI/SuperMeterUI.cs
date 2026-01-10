using UnityEngine;
using UnityEngine.UI;

public class SuperMeterUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider superMeterSlider;
    public Image fillImage; // The fill area of the slider
    public GameObject readyIndicator; // Optional: shows "READY!" text or icon

    [Header("Colors")]
    public Color normalColor = new Color(0.2f, 0.6f, 1f); // Blue when charging
    public Color readyColor = new Color(1f, 0.8f, 0f); // Gold when full

    [Header("Ready Effect")]
    public float pulseSpeed = 2f; // How fast the bar pulses when ready
    private bool isReady = false;
    private float pulseTimer = 0f;

    public void GiveEmptyMeter(float maxValue)
    {
        superMeterSlider.maxValue = maxValue;
        superMeterSlider.value = 0;
        isReady = false;
        UpdateVisuals();
    }

    public void SetMeterValue(float value)
    {
        superMeterSlider.value = value;
        
        // Check if meter is full
        isReady = value >= superMeterSlider.maxValue;
        UpdateVisuals();
    }

    public void EmptyMeter()
    {
        superMeterSlider.value = 0;
        isReady = false;
        UpdateVisuals();
    }

    void Update()
    {
        // Pulse effect when super is ready
        if (isReady && fillImage != null)
        {
            pulseTimer += Time.deltaTime * pulseSpeed;
            float pulse = (Mathf.Sin(pulseTimer) + 1f) / 2f; // 0 to 1
            fillImage.color = Color.Lerp(readyColor, Color.white, pulse * 0.3f);
        }
    }

    void UpdateVisuals()
    {
        if (fillImage != null)
        {
            fillImage.color = isReady ? readyColor : normalColor;
        }

        if (readyIndicator != null)
        {
            readyIndicator.SetActive(isReady);
        }
    }
}
