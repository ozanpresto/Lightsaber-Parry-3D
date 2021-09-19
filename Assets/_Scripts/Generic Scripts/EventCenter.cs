using UnityEngine;

public class EventCenter : MonoBehaviour
{
    public static EventCenter Instance;

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
        }
    }

    void Log(string str)
    {
        Debug.Log("[EventCenter] " + str);
    }

    #region GENERIC EVENTS

    public void OnLevelAdvanced()
    {
        Log("OnLevelAdvanced - " + (GameManager.Instance.Level));

        GameManager.Instance.AdvanceLevel();
        // Backend
        // Analytics
        // UI
        // FX
    }

    public void OnLevelFailed()
    {
        Log("OnLevelAdvanced - " + (GameManager.Instance.Level));

        GameManager.Instance.ReloadLevel();
        // Backend
        // Analytics
        // UI
        // FX
    }

    public void OnSceneLoaded(SceneName sceneName)
    {
        Log("OnSceneLoaded - " + sceneName.ToString());

        switch (sceneName)
        {
            case SceneName._preload:
                break;
            case SceneName.GameScene:
                UIManager.Instance.OpenPanel(PanelNames.MainMenu, true);
                // Backend
                // Analytics
                // UI
                // FX
                break;
        }
    }

    public void OnSceneUnloaded(SceneName sceneName)
    {
        Log("OnSceneUnloaded - " + sceneName.ToString());

        switch (sceneName)
        {
            case SceneName._preload:
                break;
            case SceneName.GameScene:
                // Backend
                // Analytics
                // UI
                // FX
                break;
        }
    }

    #endregion

}
