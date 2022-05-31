using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBoard : MonoBehaviour
{
    public static GameManagerBoard instance;

    private GameManager gameManager;

    private PlataformEvent plataformEvent;

    private PlayerController playerController;

    //Ponto
    //CheckPoint 
    //Limite Plataforma

    [Header("Route")]
    public DiceHandler dice;
    private Route route;
    [SerializeField] private int initialPlataformIndex;
    [SerializeField] private int lastPlataformIndex = 0;
    [SerializeField] private int currentPlataformIndex;
    [SerializeField] private int nextPlataformIndex;
    [SerializeField] private int goalPlataformIndex;
    public int steps;

    [Header("CheckPoint")]
    [SerializeField] private Vector3 initialPlataform;
    public Vector3 lastCheckPoint;
    public Vector3 currentPlataform;
    public Vector3 nextPlataform;
    [SerializeField] private Vector3 goalPlataform;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void Start()
    {
        //dice = FindObjectOfType<DiceHandler>();
        route = FindObjectOfType<Route>();
        gameManager = GetComponent<GameManager>();
        playerController = FindObjectOfType<PlayerController>();

    }

    public void CalculateRoute()
    {
        initialPlataform = currentPlataform;

        for (int i = 0; i < route.childNodeList.Count; i++)
        {
            if (initialPlataform == route.childNodeList[i].position)
            {
                initialPlataformIndex = i;

                CalculateNextGoal();
            }
        }
    }

    private void CalculateNextGoal()
    {
        steps = dice.DiceResult;

        goalPlataformIndex = initialPlataformIndex + dice.DiceResult;

        goalPlataform = route.childNodeList[goalPlataformIndex].position;
    }

    public void OnMoving()
    {
        // A plataforma que o dado foi rolado deve ser a InitialPlataform
        // A currentPlataform deve ser atualizada a cada nova


        for (int i = 0; i < route.childNodeList.Count; i++)
        {
            if (i == 0)
            {
                lastPlataformIndex = 0;
                lastCheckPoint = route.childNodeList[lastPlataformIndex].position;
            }


            else if (currentPlataform == route.childNodeList[i].position)
            {
                currentPlataformIndex = i;

                nextPlataformIndex = currentPlataformIndex + 1;
                nextPlataform = route.childNodeList[nextPlataformIndex].position;

                lastPlataformIndex = currentPlataformIndex - 1;
                lastCheckPoint = route.childNodeList[lastPlataformIndex].position;
            }
        }

        // Verificar se o Index da plataforma atual é maior que o anterior

        if (steps <= 0 && currentPlataform == goalPlataform)
        {
            steps = 0;

            OnReachGoal();
        }
        // Diminuir uma contagem do Numero de passos
        // Se o jogador chegar na plataforma do Index (nextGoal)
        // OnRechGoal
    }

    private void OnReachGoal()
    {
        plataformEvent = route.childNodeList[currentPlataformIndex].gameObject.GetComponent<PlataformEvent>();
        plataformEvent.OnEventStart();
    }

    public void OnEventEnd()
    {
        plataformEvent = route.childNodeList[currentPlataformIndex].gameObject.GetComponent<PlataformEvent>();
        plataformEvent.OnEventEnd();
    }

    //public void MovingPlayerToLadder(GameObject ladder, Transform nextPlataformFromLadder)
    //{
    //    Debug.LogWarning("Entrou evento GoToLadder");
        
    //    nextPlataform = nextPlataformFromLadder.position;

    //    if (currentPlataform != nextPlataform)
    //    {
    //        Debug.LogWarning("Ta subindo a escada");
    //        playerController.GoToLadder(ladder);
    //    }

    //    Debug.LogWarning("Chegou no topo");
    //    playerController.ClimbOutOfLadder(nextPlataformFromLadder);

    //    //Quando nextPlataformFromLadder for igual currentPlataform
    //    // Jogador pula fora seguindo sua direção

    //}
}
