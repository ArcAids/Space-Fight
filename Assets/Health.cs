using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, ITakeDamage
{
    [SerializeField]
    float maxHealth;

    float currentHealth;
    bool isAlive;

    public delegate void OnHealthLost(float lives);
    event OnHealthLost healthLost;
    public void RegisterHealthLostListner(OnHealthLost lost)
    {
        healthLost += lost;
    }
    public void DeRegisterHealthLostListner(OnHealthLost lost)
    {
        healthLost -= lost;
    }
    
    private void OnEnable()
    {
        isAlive = true;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!isAlive)
            return;
        currentHealth -= damage;
        healthLost?.Invoke(currentHealth);
        if(currentHealth<=0)
        {
            gameObject.SetActive(false);
            isAlive = false;
        }
    }
}
