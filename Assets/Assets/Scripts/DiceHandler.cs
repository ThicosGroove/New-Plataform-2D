using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceHandler : MonoBehaviour
{
    GameManager gameManager;
    //GameManagerBoard GMBoard;

    public Sprite[] diceSides;
    [SerializeField] private Image diceImage;

    private int _diceResult;
    public int DiceResult { get { return _diceResult; } }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        
    }

    void Start()
    {
        diceImage.sprite = diceSides[0];

        diceSides = Resources.LoadAll<Sprite>("Dice/");
    }


    public void RollButton()
    {
        StartCoroutine(DiceRollAnimation());
    }

    private IEnumerator DiceRollAnimation()
    {
        int randomDiceSide = 0;

        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);

            diceImage.sprite = diceSides[randomDiceSide];

            yield return new WaitForSeconds(0.05f);
        }

        _diceResult = randomDiceSide + 1;
        Debug.LogWarning(_diceResult);

        GameManagerBoard.instance.CalculateRoute();

        yield return new WaitForSeconds(1.5f);

        CloseDicePanel();
    }


    private void CloseDicePanel()
    {
        gameManager.CloseDicePanel();
    }
}
