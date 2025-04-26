// PlayerStats.cs
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Values")]
    public int basePower = 10;
    public int baseSpeed = 5;
    public int baseHealth = 100;

    public int currentPower { get; private set; }
    public int currentSpeed { get; private set; }
    public int currentHealth { get; private set; }
    public int maxHealth { get; private set; }

    [Header("UI (optional)")]
    public Text powerText;
    public Text speedText;
    public Text healthText;

    private void Awake()
    {
        currentPower = basePower;
        currentSpeed = baseSpeed;
        maxHealth = baseHealth;
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void AddBonus(int bonusValue, int statNumber)
    {
        print("Add");
        switch (statNumber)
        {
            case 0:
                currentPower += bonusValue;
                break;
            case 1:
                currentSpeed += bonusValue;
                break;
            case 2:
                maxHealth += bonusValue;
                currentHealth += bonusValue;
                break;
        }

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateUI();
    }

    public void RemoveBonus(int bonusValue, int statNumber)
    {
        print("Remove");
        switch (statNumber)
        {
            case 0:
                currentPower -= bonusValue;
                break;
            case 1:
                currentSpeed -= bonusValue;
                break;
            case 2:
                maxHealth -= bonusValue;
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;
                break;
        }
        UpdateUI();
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth < 0)
            currentHealth = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (powerText != null) powerText.text = $"Power: {currentPower}";
        if (speedText != null) speedText.text = $"Speed: {currentSpeed}";
        if (healthText != null) healthText.text = $"Health: {currentHealth}/{maxHealth}";
    }
}
