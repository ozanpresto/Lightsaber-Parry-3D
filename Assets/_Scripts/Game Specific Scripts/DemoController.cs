using UnityEngine;

public class DemoController : MonoBehaviour
{
    public static DemoController Instance;

    public Lightsaber frontLightsaber;
    public Lightsaber backLightsaber;

    private Colors colorsRef;
    private float changedValuesOfSliders = 0;

    private void Awake()
    {
        Instance = this;
        colorsRef = GetComponentInChildren<Colors>();
    }

    public void InitializeGamePlay()
    {
        frontLightsaber.Initialize(UIManager.Instance.frontSlider, colorsRef.colors[ConfigurableParameters.Instance.frontLightsaberColor]);
        backLightsaber.Initialize(UIManager.Instance.backSlider, colorsRef.colors[ConfigurableParameters.Instance.backLightsaberColor]);
        UIManager.Instance.simulateButton.onClick.AddListener(Simulate);
    }


    private void Simulate()
    {
        UIManager.Instance.SetInteractables(false);
        SoundManager.Instance.PlaySound(SoundTrigger.ButtonClick);
        Taptic.Light();
        frontLightsaber.Attack();
        backLightsaber.Attack();
    }

    public void SliderValuesChanged(float difference)
    {
        changedValuesOfSliders += difference;
        if(changedValuesOfSliders > ConfigurableParameters.Instance.minimumAngleChangeForNewCollisionInfo)
        {
            showInfoPopUp();
        }
    }

    private void showInfoPopUp()
    {
        changedValuesOfSliders = 0;
        UIManager.Instance.showInfoPopUp();

    }
}
