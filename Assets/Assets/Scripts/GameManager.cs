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
    [SerializeField] GameObject CardEventScreen;
    [SerializeField] GameObject DiceEventScreen;

    private Input_PlayerController input;
    private bool inputPause;

    [SerializeField] private GameState state;
    enum GameState
    {
        Play,
        Paused,
        Card,
        Dice,
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

        state = GameState.Dice;
    }

    void Update()
    {
        switch (state)
        {
            case GameState.Play:
                panelPauseMenu.SetActive(false);
                OnControlScreen.SetActive(false);
                //MobileButtons.SetActive(true);
                CardEventScreen.SetActive(false);
                //DiceEventScreen.SetActive(false);
                PressPauseOnController();
                break;
            case GameState.Paused:
                panelPauseMenu.SetActive(true);
                OnControlScreen.SetActive(false);
                //MobileButtons.SetActive(true);
                CardEventScreen.SetActive(false);
                PressPauseOnController();
                break;
            case GameState.OnControlScreen:
                //MobileButtons.SetActive(false);
                OnControlScreen.SetActive(true);
                CardEventScreen.SetActive(false);
                break;
            case GameState.Card:
                panelPauseMenu.SetActive(false);
                OnControlScreen.SetActive(false);
                //MobileButtons.SetActive(true);
                CardEventScreen.SetActive(true);
                DiceEventScreen.SetActive(false);
                break;
            case GameState.Dice:
                panelPauseMenu.SetActive(false);
                OnControlScreen.SetActive(false);
                //MobileButtons.SetActive(true);
                CardEventScreen.SetActive(false);
                DiceEventScreen.SetActive(true);
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

    public void CardEventOpen()
    {
        state = GameState.Card;
    }

    public void CardEventClose()
    {
        CardEventScreen.SetActive(false);
        state = GameState.Play;
    }


    public void OpenDicePanel()
    {
        state = GameState.Dice;
    }

    public void CloseDicePanel()
    {
       DiceEventScreen.SetActive(false);
        state = GameState.Play;
    }



    //public void GoToNextLevel()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //}
}
