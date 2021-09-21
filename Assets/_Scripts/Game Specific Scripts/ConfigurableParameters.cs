using UnityEngine;

public class ConfigurableParameters : MonoBehaviour
{
    public static ConfigurableParameters Instance;

    public ColorName frontLightsaberColor;
    public ColorName backLightsaberColor;

    [Range(1.0f, 2.0f)]
    public float simulationSpeed = 1f;
    [Range(1.0f, 20.0f)]
    public float minimumAngleChangeForNewCollisionInfo = 1f;

    private void Awake()
    {
        Instance = this;
    }
}
