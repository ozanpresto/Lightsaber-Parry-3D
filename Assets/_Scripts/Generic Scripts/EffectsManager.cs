using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public enum EffectTrigger
{
    Hit,
    Start,
}

[System.Serializable]
public class Effects : SerializableDictionaryBase<EffectTrigger, GameObject> { }

public class EffectsManager : MonoBehaviour
{
    [Header("This script requires CameraEffects Layer and an EffectsCamera Tag")]
    public Effects effects;
    public static EffectsManager Instance;
    private Camera effectsCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        effectsCamera = GameObject.FindGameObjectWithTag(Tags.EFFECTS_CAMERA).GetComponent<Camera>();
    }


    public void PlayEffect(EffectTrigger effectTrigger)
    {
        var effect = Instantiate(effects[effectTrigger], effectsCamera.transform);
        effect.transform.localPosition = Vector3.forward * 3;
        effect.layer = LayerMask.NameToLayer("CameraEffects");

        foreach (Transform child in effect.transform)
            child.gameObject.layer = LayerMask.NameToLayer("CameraEffects");

        effect.AddComponent<SelfDestruct>().lifetime = 2f;

        //for special behavior
        switch (effectTrigger)
        {
            case EffectTrigger.Hit:
                break;
            case EffectTrigger.Start:
                break;
        }
    }

    public void PlayEffectAtPos(EffectTrigger effectTrigger, Vector3 pos, Vector3 euler, Transform parent = null)
    {
        var effect = Instantiate(effects[effectTrigger], pos, Quaternion.Euler(euler), parent);
        effect.AddComponent<SelfDestruct>().lifetime = 2f;

        //for special behavior
        switch (effectTrigger)
        {
            case EffectTrigger.Hit:
                break;
            case EffectTrigger.Start:
                break;
        }
    }

}