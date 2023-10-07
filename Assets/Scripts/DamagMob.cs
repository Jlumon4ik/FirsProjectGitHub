using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DamagMob : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Text lostText;


    public HPBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            TakeDamage(25);

            if (currentHealth == 0)
            {
                Destroy(gameObject);
                DisplayVictoryMessage();
            }

        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    private void DisplayVictoryMessage()
    {
        lostText.text = "Вы проиграли!";
    }

}
