using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;

    private void Start()
    {
        boxCollider2D = GetComponentInChildren<BoxCollider2D>();

        boxCollider2D.isTrigger = true;
    }


    private void TakeHit() 
    {
        Die();
    }


    public void Die()
    {
        transform.position = GameManagerBoard.instance.lastCheckPoint + Vector3.up * 3f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<AEnemy>())
        {
            TakeHit();
        }

        if (collision.gameObject.CompareTag("Death"))
        {
            Die();
        }

        if (collision.gameObject.CompareTag("Treasure"))
        {
           //GoToNextLevel();
        }
    }
}
