using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureWinScript : MonoBehaviour
{
     public GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("Entrou");

        if (collision.CompareTag("Player"))
        {
            Debug.LogWarning("Vai pro proximo");

            //gameManager.GoToNextLevel();
        }
    }
}
