using System.Collections;
using UnityEngine;

// Скрипт для огненного следа от нот
public class FireTrail : MonoBehaviour
{
    public float duration = 3f;
    public float damagePerSecond = 5f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var other = collision.collider;
        if (other.CompareTag("Player"))
        {
            int damageToApply = Mathf.CeilToInt(damagePerSecond * Time.deltaTime);
            if (damageToApply > 0)
                other.GetComponent<PlayerBhvr>()?.TakeDamage(damageToApply);
        }
    }
}
