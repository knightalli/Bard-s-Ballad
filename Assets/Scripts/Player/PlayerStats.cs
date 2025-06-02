// PlayerStats.cs
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Values")]
    public float basePower = 10;
    public float baseCooldown = 1;
    public float baseHealth = 100;

    public float currentPower;
    public float currentCooldown;
    public float currentHealth;
    public float maxHealth;


    private void Awake()
    {
        currentPower = basePower;
        currentCooldown = baseCooldown;
        maxHealth = baseHealth;
        currentHealth = maxHealth;
    }

    public void AddBonus(float bonusValue, float statNumber)
    {
        print("Add");
        switch (statNumber)
        {
            case 0:
                currentPower += bonusValue;
                break;
            case 1:
                currentCooldown -= bonusValue;
                break;
            case 2:
                maxHealth += bonusValue;
                currentHealth += bonusValue;
                break;
        }

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void RemoveBonus(float bonusValue, float statNumber)
    {
        print("Remove");
        switch (statNumber)
        {
            case 0:
                currentPower -= bonusValue;
                break;
            case 1:
                currentCooldown += bonusValue;
                break;
            case 2:
                maxHealth -= bonusValue;
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;
                break;
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth < 0)
            currentHealth = 0;
    }
}
