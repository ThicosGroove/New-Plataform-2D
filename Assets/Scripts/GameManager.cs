using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject panelPauseMenu;
    [SerializeField] GameObject OnControlScreen;
    [SerializeField] GameObject MobileButtons;

    private Input_PlayerController input;
    private bool inputPause;

    private GameState state;
    enum GameState
    {
        Play,
        Paused,
        OnControlScreen
    }

    private void Awake()
    {
        input = new Input_PlayerController();      
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void Start()
    {
        if (GameSavingData.Instance != null)
        {
            GameSavingData.Instance._level = SceneManager.GetActiveScene().buildIndex;

            GameSavingData.Instance.SaveNewData();
        }  

        state = GameState.Play;
    }

    void Update()
    {
        switch (state)
        {
            case GameState.Play:
                panelPauseMenu.SetActive(false);
                OnControlScreen.SetActive(false);
                MobileButtons.SetActive(true);
                PressPauseOnController();
                break;
            case GameState.Paused:
                panelPauseMenu.SetActive(true);
                OnControlScreen.SetActive(false);
                MobileButtons.SetActive(true);
                PressPauseOnController();
                break;
            case GameState.OnControlScreen:
                MobileButtons.SetActive(false);
                OnControlScreen.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void PressPauseOnController()
    {
        inputPause = input.Player.PauseController.WasPressedThisFrame();

        if (inputPause)
        {
            if (state == GameState.Play)
            {
                PauseButton();
            }
            else if (state == GameState.Paused)
            {
                ResumeButton();
            }
        }
    }

    public void PauseButton()
    {
        Time.timeScale = 0;
        state = GameState.Paused;

        panelPauseMenu.SetActive(true);
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        state = GameState.Play;

        panelPauseMenu.SetActive(false);
    }

    public void ControlButton()
    {
        state = GameState.OnControlScreen;

        OnControlScreen.SetActive(true);
    }

    public void BackFromControlButton()
    {
        OnControlScreen.SetActive(false);

        state = GameState.Paused;
    }

    public void QuitButton()
    {
        //Got to main menu
        SceneManager.LoadScene(0);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
