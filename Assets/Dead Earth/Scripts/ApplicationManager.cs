using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameState
{
    public string Key = null;
    public string Value = null;
}
public class ApplicationManager : MonoBehaviour
{
    [SerializeField] private List<GameState> _startingGameStates = new List<GameState>();
    private Dictionary<string, string> _gameStateDictionary = new Dictionary<string, string>();
    //Singleton Design
    private static ApplicationManager _Instance = null;

    public static ApplicationManager instance
    {
        get
        {
            if(_Instance==null) { _Instance = (ApplicationManager)FindObjectOfType(typeof(ApplicationManager)); }
            return _Instance;
        }
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ResetGameStates();
    }
    void ResetGameStates()
    {
        _gameStateDictionary.Clear();
        for (int i = 0; i < _startingGameStates.Count; i++)
        {
            GameState gs = _startingGameStates[i];
            _gameStateDictionary[gs.Key] = gs.Value;
        }
    }
    public string GetGameState(string key)
    {
        string result = null;
        _gameStateDictionary.TryGetValue(key, out result);
        return result;
    }
    public bool SetGameState(string key, string value)
    {
        if (key == null || value == null) return false;
        _gameStateDictionary[key] = value;
        return true;
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void LoadGame()
    {
        ResetGameStates();
        SceneManager.LoadScene("The Game");
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
