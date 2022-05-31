using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    GameManager gameManager;

    public Image corretButton;
    public Image wrongButton;

    [SerializeField] private Color correctButtonColor;
    [SerializeField] private Color wrongButtonColor;


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        corretButton.color = Color.white;
        wrongButton.color = Color.white;      
    }

    public void CorrectAnswer()
    {
        //Dar ponto ao gameManagerBoard

        ColorButtons();
    }

    public void WrongAnswer()
    {
        ColorButtons();       
    }

    private void ColorButtons()
    {
        corretButton.color = correctButtonColor;
        wrongButton.color = wrongButtonColor;

        StartCoroutine(CallCardEventClose());
    }

    private IEnumerator CallCardEventClose()
    {
        yield return new WaitForSeconds(1.5f);

        gameManager.CardEventClose();
    }
}
