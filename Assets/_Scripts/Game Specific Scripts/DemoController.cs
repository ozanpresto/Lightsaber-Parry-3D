using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoController : MonoBehaviour
{
    public static DemoController Instance;

    public Lightsaber frontLightsaber;
    public Lightsaber backLightsaber;

    private Colors colorsRef;


    private void Awake()
    {
        Instance = this;
        colorsRef = GetComponentInChildren<Colors>();
    }

    public void InitializeGamePlay()
    {
        frontLightsaber.Initialize(UIManager.Instance.frontSlider, colorsRef.colors[ConfigurableParameters.Instance.frontLightsaberColor], true);
        backLightsaber.Initialize(UIManager.Instance.backSlider, colorsRef.colors[ConfigurableParameters.Instance.backLightsaberColor], false);
        UIManager.Instance.OpenAndCloseInteractablesParent();
        UIManager.Instance.simulateButton.onClick.AddListener(Simulate);
    }


    public void Simulate()
    {
        frontLightsaber.Attack();
        backLightsaber.Attack();
    }
}
