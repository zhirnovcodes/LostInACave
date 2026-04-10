using UnityEngine;
using UnityEngine.UI;

public class BatteryElement : MonoBehaviour
{
    public Slider BatterySlider;
    public Image LowBatteryIcon;

    private const float LowBatteryThreshold = 5f;

    public void SetLevel(float level)
    {
        BatterySlider.value = level / 100f;
        LowBatteryIcon.enabled = level < LowBatteryThreshold;
    }
}
