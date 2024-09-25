using UnityEngine;
using UnityEngine.UI;

public class FillColorChanger : MonoBehaviour
{
    public Image fillImage;  // Reference to your UI Image
    public Color fullColor = Color.green;  // The color when fillAmount is 1 (green)
    public Color middleColor = Color.yellow; // The color for a middle point (yellow)
    public Color emptyColor = Color.red;  // The color when fillAmount is 0 (red)

    void Update()
    {
        UpdateColor(fillImage.fillAmount);
    }

    void UpdateColor(float fillAmount)
    {
        // Interpolating between Green -> Yellow -> Red based on fillAmount
        if (fillAmount > 0.5f)
        {
            // Lerp from Green to Yellow
            fillImage.color = Color.Lerp(middleColor, fullColor, (fillAmount - 0.5f) * 2f);
        }
        else
        {
            // Lerp from Yellow to Red
            fillImage.color = Color.Lerp(emptyColor, middleColor, fillAmount * 2f);
        }
    }
}
