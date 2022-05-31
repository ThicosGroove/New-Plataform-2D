using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformEvent : MonoBehaviour
{
    GameManager gameManager;
    public GameObject Ladder;
    public Transform nextPlataformFromLadder;

    //Serão 4 eventos
    //Carta
    //Escada
    //Escorregador
    //Lata de lixo

    [SerializeField] PlataformCurrentEventState eventState;
    public enum PlataformCurrentEventState
    {
        Default,
        CardEvent,
        LadderEvent,
        SliderEvent,
        TrashCanEvent
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }


    public void OnEventStart()
    {
        switch (eventState)
        {
            case PlataformCurrentEventState.Default:
                gameManager.OpenDicePanel();
                break;
            case PlataformCurrentEventState.CardEvent:
                gameManager.CardEventOpen();
                break;
            case PlataformCurrentEventState.LadderEvent:              
                OnLadderEvent();
                break;
            case PlataformCurrentEventState.SliderEvent:
                break;
            case PlataformCurrentEventState.TrashCanEvent:
                break;
            default:
                gameManager.OpenDicePanel();
                break;
        }
    }

    public void OnEventEnd()
    {
        gameManager.OpenDicePanel();
    }


    private void OnLadderEvent()
    {
        if (Ladder != null && nextPlataformFromLadder != null)
        {
            Debug.LogWarning("Começou evento Escada");
            //GameManagerBoard.instance.MovingPlayerToLadder(Ladder, nextPlataformFromLadder);
        }
    }
}
