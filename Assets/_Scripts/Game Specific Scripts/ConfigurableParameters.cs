using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurableParameters : MonoBehaviour
{
    public static ConfigurableParameters Instance;

    public ColorName frontLightsaberColor;
    public ColorName backLightsaberColor;

    public float simulationSpeed = 1f;
    public float minimumAngleChangeForNewCollisionInfo = 1f;

    private void Awake()
    {
        Instance = this;
    }
}
