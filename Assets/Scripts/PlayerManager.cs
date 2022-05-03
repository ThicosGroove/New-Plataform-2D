using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public GameManager gameManager;

    private BoxCollider2D boxCollider2D;

    [SerializeField] private Image PlayerFilledHealthBar;
    [SerializeField] private float playerMaxHeatlh;
    private float currentHealth;


    private void Start()
    {
        currentHealth = playerMaxHeatlh;
        boxCollider2D = GetComponentInChildren<BoxCollider2D>();
    }

    private void Update()
    {
        HealthBarFiller();
    }

    private void LostHealth()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void HealthBarFiller()
    {
        float fillAmountPercentage = currentHealth / playerMaxHeatlh;
        float lerpSpeed = 4f * Time.deltaTime;

        PlayerFilledHealthBar.fillAmount = Mathf.Lerp(PlayerFilledHealthBar.fillAmount, fillAmountPercentage, lerpSpeed);
    }

    private void Die()
    {
        gameManager.ReloadScene();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<AEnemy>() && this.boxCollider2D)
        {
            Debug.LogWarning("Tomou dano inimigo");
            LostHealth();
        }

        if (collision.gameObject.CompareTag("Death"))
        {
            Die();
        }

    }

}
