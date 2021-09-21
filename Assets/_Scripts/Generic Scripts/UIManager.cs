using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.UI;
using System;
using DG.Tweening;

public enum PanelNames
{
    MainMenu,
    InGame,
    Settings,
}

[System.Serializable]
public class UIPanels : SerializableDictionaryBase<PanelNames, UIPanelAndSetup> { }

[System.Serializable]
public class UIPanelAndSetup
{
    public GameObject UIPanel;
    public UnityEvent UIPanelSetup;
}

public class UIManager : MonoBehaviour
{
    public UIPanels UIPanelsDictionary;

    [Header("INGAME ITEMS")]
    public Slider frontSlider;
    public Slider backSlider;
    public GameObject InteractablesParent;
    public Button simulateButton;
    public GameObject infoPopUpPrefab;
    private bool isInteractablesOpen = true;
    private GameObject Info = null;


    [Header("MAIN MENU ITEMS")]
    public Image vibrationImage;
    public Image soundImage;
    public Sprite VonSprite;
    public Sprite VoffSprite;
    public Sprite SonSprite;
    public Sprite SoffSprite;

    public static UIManager Instance;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void ToggleVibration()
    {
        GameManager.Instance.VibrationOn = !GameManager.Instance.VibrationOn;
        vibrationImage.sprite = GameManager.Instance.VibrationOn ? VonSprite : VoffSprite;
    }
    public void ToggleSound()
    {
        GameManager.Instance.SoundOn = !GameManager.Instance.SoundOn;
        soundImage.sprite = GameManager.Instance.SoundOn ? SonSprite : SoffSprite;
    }

    #region Panel Functions

    public void OpenPanel(string panel)
    {
        PanelNames panelName;
        if (Enum.TryParse<PanelNames>(panel, out panelName))
            OpenPanel(panelName);
        else
            Debug.LogWarning("Did not find panel: " + panel);
    }

    public void OpenPanel(PanelNames panelName, bool closeOtherPanels)
    {
        UIPanelAndSetup panelToOpen;
        if (UIPanelsDictionary.TryGetValue(panelName, out panelToOpen))
        {

            if (closeOtherPanels)
            {
                CloseAllPanels();
            }

            panelToOpen.UIPanel.SetActive(true);

            if (panelToOpen.UIPanelSetup != null)
            {
                panelToOpen.UIPanelSetup.Invoke();
            }

        }
        else
        {
            Debug.LogWarning("No value for key: " + panelName + " exists");
        }
    }


    public void OpenPanel(PanelNames[] names, bool closeOtherPanels)
    {
        if (closeOtherPanels)
            CloseAllPanels();

        foreach (PanelNames panelName in names)
            OpenPanel(panelName, false);
    }

    public void OpenPanel(PanelNames name, bool closeOtherPanels, float delay)
    {
        StartCoroutine(AddDelay(delay, () => { OpenPanel(name, closeOtherPanels); }));
    }

    public void OpenPanel(PanelNames panelName)
    {
        UIPanelAndSetup panelToOpen;
        if (UIPanelsDictionary.TryGetValue(panelName, out panelToOpen))
        {
            panelToOpen.UIPanel.SetActive(true);
            panelToOpen.UIPanelSetup?.Invoke();
        }
        else
        {
            Debug.LogWarning("No value for key: " + panelName + " exists");
        }

    }

    public void ClosePanel(string panel)
    {
        PanelNames panelName;
        if (!Enum.TryParse<PanelNames>(panel, out panelName))
        {
            Debug.LogWarning("No enum for string: " + panel);
            return;
        }

        UIPanelAndSetup currentPanel;
        if (UIPanelsDictionary.TryGetValue(panelName, out currentPanel))
            currentPanel.UIPanel.SetActive(false);
    }

    public void ClosePanel(PanelNames panelName)
    {
        UIPanelAndSetup currentPanel;
        if (UIPanelsDictionary.TryGetValue(panelName, out currentPanel))
            currentPanel.UIPanel.SetActive(false);
    }


    void CloseAllPanels()
    {
        foreach (PanelNames panelName in UIPanelsDictionary.Keys)
            ClosePanel(panelName);
    }

    IEnumerator AddDelay(float xSeconds, UnityAction Action)
    {
        yield return new WaitForSecondsRealtime(xSeconds);
        Action();
    }



    #endregion


    #region Game Spesific Functions

    public void SetupSettings()
    {
        soundImage.sprite = GameManager.Instance.SoundOn ? SonSprite : SoffSprite;
        vibrationImage.sprite = GameManager.Instance.VibrationOn ? VonSprite : VoffSprite;
    }

    public void SettingsButtonOnClick()
    {
        UIPanelAndSetup temp;
        if (UIPanelsDictionary.TryGetValue(PanelNames.Settings, out temp))
        {
            if (temp.UIPanel.activeInHierarchy)
                ClosePanel(PanelNames.Settings);
            else
                OpenPanel(PanelNames.Settings);
        }
    }

    public void StartGamePlay()
    {
        OpenPanel(PanelNames.InGame, true);
        OpenAndCloseInteractablesParent();
        EffectsManager.Instance.PlayEffect(EffectTrigger.Start);
        DemoController.Instance.InitializeGamePlay();
    }

    public void OpenAndCloseInteractablesParent()
    {
        if (isInteractablesOpen)
        {
            isInteractablesOpen = false;
            InteractablesParent.transform.DOComplete();
            InteractablesParent.transform.DOLocalMoveY(-600, 0.1f).SetRelative(true).SetEase(Ease.Linear);
        }
        else
        {
            isInteractablesOpen = true;
            InteractablesParent.transform.DOComplete();
            InteractablesParent.transform.DOLocalMoveY(600, 1f).SetRelative(true).SetDelay(0.25f).SetEase(Ease.Linear);
        }
    }

    public void SetInteractables(bool isInteractable)
    {
        simulateButton.interactable = isInteractable;
        backSlider.interactable = isInteractable;
        frontSlider.interactable = isInteractable;
    }

    public void showInfoPopUp()
    {
        if (Info != null)
        {
            Info.GetComponent<Image>().DOFade(0f, 0.5f);
            Info.GetComponentInChildren<TMPro.TMP_Text>().DOFade(0f, 0.5f);
        }

        Transform parent = null;
        UIPanelAndSetup temp;
        if (UIPanelsDictionary.TryGetValue(PanelNames.InGame, out temp))
        {
            parent = temp.UIPanel.transform;
        }

        Info = Instantiate(infoPopUpPrefab, parent);
        Info.transform.localEulerAngles = Vector3.zero;
        Info.transform.localPosition = new Vector3(0, 600, 0);
        Info.transform.localScale = Vector3.one * 1.5f;
        if (GetSumOfSliderValeus() > 32)
            Info.GetComponentInChildren<TMPro.TMP_Text>().text = "Will Collide";
        else
            Info.GetComponentInChildren<TMPro.TMP_Text>().text = "Wont Collide";

        Info.transform.DOScale(Vector3.zero, 0.7f).SetDelay(0.7f);
        Info.transform.DOLocalMoveY(500, 1.4f).SetRelative(true).SetEase(Ease.Linear);
        Info.AddComponent<SelfDestruct>().lifetime = 1.5f;
    }

    public float GetSumOfSliderValeus()
    {
        return backSlider.value + frontSlider.value;
    }

    #endregion

}

