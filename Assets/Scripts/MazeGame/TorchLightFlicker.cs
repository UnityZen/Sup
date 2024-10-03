using UnityEngine;
using UnityEngine.Rendering.Universal;  // For Light2D

public class TorchLightFlicker : MonoBehaviour
{
    public Light2D torchLight;                 // Reference to the 2D Light component
    public float intensityMin = 0.8f;          // Minimum intensity for the torch light
    public float intensityMax = 1.2f;          // Maximum intensity for the torch light
    public float flickerSpeed = 0.2f;          // Speed of the flicker effect (lower = faster flicker)
    public float smoothFlickerSpeed = 5f;      // Smooth speed for intensity change

    public Color minColor = Color.yellow;      // Minimum color (warmer, more orange/red)
    public Color maxColor = Color.white;       // Maximum color (whiter, brighter)
    public float colorChangeSpeed = 1f;        // Speed at which the color changes

    private float targetIntensity;             // Target intensity to lerp towards
    private float colorTimer = 0f;             // Timer to change colors

    void Start()
    {
        if (torchLight == null)
        {
            torchLight = GetComponent<Light2D>(); // Auto-assign the Light2D component
        }

        // Initialize the target intensity
        targetIntensity = Random.Range(intensityMin, intensityMax);
    }

    void Update()
    {
        FlickerLight();
        ChangeLightColor();
    }

    void FlickerLight()
    {
        // Smoothly adjust the intensity towards the target intensity
        torchLight.intensity = Mathf.Lerp(torchLight.intensity, targetIntensity, Time.deltaTime * smoothFlickerSpeed);

        // Randomly pick a new target intensity at intervals
        if (Mathf.Abs(torchLight.intensity - targetIntensity) < 0.01f)
        {
            targetIntensity = Random.Range(intensityMin, intensityMax);
        }
    }

    void ChangeLightColor()
    {
        // Smoothly change between the minColor and maxColor over time for a dynamic torch effect
        colorTimer += Time.deltaTime * colorChangeSpeed;
        torchLight.color = Color.Lerp(minColor, maxColor, Mathf.PingPong(colorTimer, 1f));
    }
}
