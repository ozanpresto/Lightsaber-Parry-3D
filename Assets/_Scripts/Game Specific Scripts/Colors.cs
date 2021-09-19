using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class Colors : MonoBehaviour
{
    public ColorNameToColor colors;
}

public enum ColorName
{
    Red,
    Green,
    Blue,
    Orange,
}

[System.Serializable]
public class aColor
{
    public Color lightsaberColor;
    public Color sliderBgColor;
}

[System.Serializable]
public class ColorNameToColor : SerializableDictionaryBase<ColorName, aColor> { }

