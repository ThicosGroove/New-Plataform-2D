using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlataformCheckPoint : MonoBehaviour
{
    private bool hasReach = false;
    public GameObject barrierChild;

    private void Update()
    {
        UpdateBarrier();
    }

    private void UpdateBarrier()
    {
        if (this.transform.position == GameManagerBoard.instance.currentPlataform)
        {
            barrierChild.SetActive(true);
        }
        else
        {
            barrierChild.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.LogWarning("Entrou");
            GameManagerBoard.instance.currentPlataform = transform.position;

            if (!hasReach)
            {
                GameManagerBoard.instance.steps--;

                hasReach = true;
            }

            GameManagerBoard.instance.OnMoving();
        }
    }
}
