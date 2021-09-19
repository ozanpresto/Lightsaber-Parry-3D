using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;


public class Lightsaber : MonoBehaviour
{
    public GameObject lightPart;
    private new Collider collider;
    private Slider connectedSlider;
    private bool isFront;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Tags.PLAYER))
        {
            SetColliderEnabled(false);
            var pos = collision.contacts[0].point;
            EffectsManager.Instance.PlayEffectAtPos(EffectTrigger.Hit, pos, Vector3.zero);
            SoundManager.Instance.PlaySound(SoundTrigger.Collision);
        }
    }

    public void Initialize(Slider slider, aColor color, bool _isFront)
    {
        collider = GetComponentInChildren<Collider>();
        connectedSlider = slider;
        connectedSlider.onValueChanged.AddListener(delegate { connectedSliderValueChanged(); });
        SetColor(color);
        isFront = _isFront;
        InitialAnim();
    }

    private void connectedSliderValueChanged()
    {
        Debug.Log(connectedSlider.value);
    }

    private void InitialAnim()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 90), 1f);
        lightPart.transform.DOScale(Vector3.one, 1f);
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
    }
}
