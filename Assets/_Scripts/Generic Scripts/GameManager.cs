using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.SceneManagement;
using System.Collections;


public enum SceneName
{
    _preload,
    GameScene
}

[System.Serializable]
public class Scenes : SerializableDictionaryBase<SceneName, int> { }

public class GameManager : MonoBehaviour
{
    public Scenes Scenes;
    private SceneName currentScene;

    #region Generic Properties

    private bool _soundOn;
    private bool _vibrationOn;

    public int Level
    {
        get
        {
            return PlayerPrefs.GetInt("level", 0);
        }
        set
        {
            PlayerPrefs.SetInt("level", value);
            PlayerPrefs.Save();
        }
    }

    public bool SoundOn
    {
        get
        {
            if (PlayerPrefs.HasKey("SoundOn"))
                return PlayerPrefs.GetInt("SoundOn") != 0;
            else
            {
                SoundOn = true;
                return true;
            }
        }
        set
        {
            PlayerPrefs.SetInt("SoundOn", (value ? 1 : 0));
            PlayerPrefs.Save();
            _soundOn = value;
            AudioListener.pause = !_soundOn;
        }
    }

    public bool VibrationOn
    {
        get
        {
            if (PlayerPrefs.HasKey("VibrationOn"))
                return PlayerPrefs.GetInt("VibrationOn") != 0;
            else
            {
                VibrationOn = true;
                return true;
            }

        }
        set
        {
            PlayerPrefs.SetInt("VibrationOn", (value ? 1 : 0));
            PlayerPrefs.Save();
            _vibrationOn = value;
            Taptic.tapticOn = _vibrationOn;

        }
    }

    #endregion

    #region Generic Methods
    public void LoadSceneAsync(SceneName sceneName)
    {
        int tempID = -100;

        if (Scenes.TryGetValue(sceneName, out tempID))
            StartCoroutine(LoadMySceneAsync(sceneName));
    }

    IEnumerator LoadMySceneAsync(SceneName sceneName)
    {

        EventCenter.Instance.OnSceneUnloaded(currentScene);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString());

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        currentScene = sceneName;
        // Initialize Scene Managers

        EventCenter.Instance.OnSceneLoaded(currentScene);

    }

    public void AdvanceLevel()
    {
        Level++;
        ReloadLevel();
        Debug.Log("Advancing to Level: " + Level);
    }

    public void DecreaseLevel()
    {
        Level--;
        ReloadLevel();
        Debug.Log("Decreasing Level to: " + Level);
    }

    public void ReloadLevel()
    {
        Debug.Log("Reload Level " + Level);
        LoadSceneAsync(currentScene);
    }

    public bool IsScene(SceneName sceneName)
    {
        return currentScene == sceneName;
    }
    #endregion

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        currentScene = SceneName._preload;
    }

    private void Start()
    {
        AudioListener.pause = !SoundOn;
        Taptic.tapticOn = VibrationOn;

        LoadSceneAsync(SceneName.GameScene);
    }
}
