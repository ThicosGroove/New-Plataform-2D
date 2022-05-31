using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerCreditsScene : MonoBehaviour
{
    private void Start()
    {
        if (GameSavingData.Instance != null)
        {
            GameSavingData.Instance._level = SceneManager.GetActiveScene().buildIndex;

            GameSavingData.Instance.SaveNewData();
        }
    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene(0);
    }
}
