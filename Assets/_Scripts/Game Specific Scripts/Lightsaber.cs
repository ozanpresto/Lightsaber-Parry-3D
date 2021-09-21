using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Lightsaber : MonoBehaviour
{
    public GameObject lightPart;
    private new Collider collider;
    private Slider connectedSlider;
    private float oldSliderValue;

    private void Start()
    {
        lightPart.transform.localScale = new Vector3(1, 0, 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag(Tags.PLAYER))
        {
            SetColliderEnabled(false);
            transform.DOKill();
            transform.DOLocalRotate(new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z), 0.5f / ConfigurableParameters.Instance.simulationSpeed).SetEase(Ease.Linear).OnComplete(() =>
            {
                UIManager.Instance.SetInteractables(true);
                SetColliderEnabled(false);
            });
            var pos = collision.contacts[0].point;
            EffectsManager.Instance.PlayEffectAtPos(EffectTrigger.Hit, pos, Vector3.zero);
            Taptic.Medium();
            SoundManager.Instance.PlaySound(SoundTrigger.Collision);
        }
    }

    public void Initialize(Slider slider, aColor color)
    {
        collider = GetComponent<Collider>();
        SetColliderEnabled(false);
        connectedSlider = slider;
        connectedSlider.onValueChanged.AddListener(delegate { connectedSliderValueChanged(); });
        SetColor(color);
        oldSliderValue = connectedSlider.value;

        InitialAnim();
    }

    private void connectedSliderValueChanged()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, connectedSlider.value);
        float temp = Mathf.Abs(oldSliderValue - connectedSlider.value);
        oldSliderValue = connectedSlider.value;
        DemoController.Instance.SliderValuesChanged(temp);
    }

    private void InitialAnim()
    {
        transform.DOLocalRotate(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0), 1f);
        lightPart.transform.DOScale(Vector3.one, 1f).SetEase(Ease.Linear);
    }

    private void SetColor(aColor color)
    {
        lightPart.GetComponent<Renderer>().material.color = color.lightsaberColor;
        lightPart.GetComponent<Renderer>().material.SetColor("_EmissionColor", color.lightsaberColor);
        connectedSlider.transform.GetChild(0).GetComponent<Image>().color = color.sliderBgColor;
    }

    private void SetColliderEnabled(bool isOpen)
    {
        if(isOpen)
            collider.enabled = true;
        else
            collider.enabled = false;
    }

    public void Attack()
    {
        SetColliderEnabled(true);
        transform.DOLocalRotate(new Vector3(60, 0, 0), 1.2f / ConfigurableParameters.Instance.simulationSpeed).SetEase(Ease.InBack).SetRelative(true).OnComplete(() =>
        {
            transform.DOLocalRotate(new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z), 1f / ConfigurableParameters.Instance.simulationSpeed).SetEase(Ease.Linear).OnComplete(() =>
            {
                UIManager.Instance.SetInteractables(true);
                SetColliderEnabled(false);
            });
        });
    }
}
