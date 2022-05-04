using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject panelPauseMenu;

    private Input_PlayerController input;
    private float inputPause;

    private GameState state;
    enum GameState
    {
        Play,
        Paused
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
        GameSavingData.Instance._level = SceneManager.GetActiveScene().buildIndex;

        GameSavingData.Instance.SaveNewData();

        state = GameState.Play;
        panelPauseMenu.SetActive(false);
    }

    void Update()
    {
        PressPauseOnController();
    }

    private void PressPauseOnController()
    {
        inputPause = input.Player.PauseController.ReadValue<float>();

        if (inputPause > 0)
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
        if (state == GameState.Play)
        {
            Time.timeScale = 0;
            state = GameState.Paused;

            panelPauseMenu.SetActive(true);
        }
    }

    public void ResumeButton()
    {
        if (state == GameState.Paused)
        {
            Time.timeScale = 1;
            state = GameState.Play;

            panelPauseMenu.SetActive(false);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
