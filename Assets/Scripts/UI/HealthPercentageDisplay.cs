using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthPercentageDisplay : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Image _healthSprite;
    private float _lastHealth = -1f;

    private void Update()
    {
        // Обновляем только при изменении здоровья
        if (!Mathf.Approximately(playerStats.currentHealth, _lastHealth))
        {
            _lastHealth = playerStats.currentHealth;
            int pct = Mathf.RoundToInt(playerStats.currentHealth / playerStats.maxHealth * 100f);
            _healthText.text = pct + "%";

            if (playerStats.currentHealth < playerStats.maxHealth * 0.25 && _healthText.color != Color.red)
            {
                _healthText.color = Color.red;
                _healthSprite.color = Color.red;
            }

            if (playerStats.currentHealth >= playerStats.maxHealth * 0.25 && _healthText.color == Color.red)
            {
                _healthText.color = Color.cyan;
                _healthSprite.color = Color.white;
            }
        }
    }
}
