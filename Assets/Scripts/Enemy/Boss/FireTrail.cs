using UnityEngine;

public class FireTrail : MonoBehaviour
{
    [Tooltip("Сколько секунд живёт след")]
    public float duration = 3f;
    [Tooltip("Урон в секунду")]
    public float damagePerSecond = 5f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    // Срабатывает, когда любой Rigidbody2D входит в триггер
    private void OnTriggerStay2D(Collider2D other)
    {
        // Попадаем только по игроку через поиск компонента
        if (other.TryGetComponent<PlayerBhvr>(out var player))
        {
            int dmg = Mathf.CeilToInt(damagePerSecond * Time.deltaTime);
            if (dmg > 0)
                player.TakeDamage(dmg);
        }
    }
}