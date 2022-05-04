using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuGameManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject OnFirstPlay;
    [SerializeField] GameObject OnLoadScreen;

    [Header("On First Play Texts")]
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text errorMessageFirstPlay;

    [Header("On Load Screen Texts")]
    [SerializeField] TMP_Text loadPlayerName;
    [SerializeField] TMP_Text loadPlayerLevel;
    [SerializeField] TMP_Text errorMessageToPlayer;


    private State state;
    enum State
    {
        MainMenu,
        OnFirstPlay,
        OnLoadScreen
    }

    private void Start()
    {
        state = State.MainMenu;

        loadPlayerName.text = "Player: " + GameSavingData.Instance._name;
        loadPlayerLevel.text = "Current Level: " + GameSavingData.Instance._level;
        errorMessageToPlayer.text = "";
        errorMessageFirstPlay.text = "";

        Debug.LogWarning(Application.persistentDataPath);
        
    }

    private void Update()
    {
        switch (state)
        {
            case State.MainMenu:
                MainMenu.SetActive(true);
                OnFirstPlay.SetActive(false);
                OnLoadScreen.SetActive(false);
                break;
            case State.OnFirstPlay:
                MainMenu.SetActive(false);
                OnFirstPlay.SetActive(true);
                OnLoadScreen.SetActive(false);
                break;
            case State.OnLoadScreen:
                MainMenu.SetActive(false);
                OnFirstPlay.SetActive(false);
                OnLoadScreen.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ReturnToMenuButton()
    {
        state = State.MainMenu;
    }

    public void MainMenuPlayButton()
    {
        state = State.OnFirstPlay;
    }

    public void MainMenuLoadButton()
    {
        state = State.OnLoadScreen;
    }

    public void KeepCurrentName()
    {
        string s;
        s = inputField.text;
        GameSavingData.Instance._name = s;
    }

    public void PlayOnLoadButton()
    {
        if (GameSavingData.Instance._level != 0 && GameSavingData.Instance._name != null)
        {
            SceneManager.LoadScene(GameSavingData.Instance._level);
            Debug.LogWarning(GameSavingData.Instance._level);
        }
        else
        {
            errorMessageToPlayer.text = "You have no save files to load";
        }
    }

    public void FirstPlayThroughButton()
    {
        if (inputField.text != "")
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            errorMessageFirstPlay.text = "You have to enter a name to Play";
        }
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

  
}
