using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float lifetime = 1f;              // Время жизни объекта
    public float damage = 20f;               // Урон цели
    public float radius = 2f;                // Радиус взрыва

    [Header("VFX")]
    public ParticleSystem particles;         // Префаб или компонент Particle System

    private void Start()
    {
        // Если вы захотите запустить частицы программно:
        if (particles != null)
            particles.Play();

        // Уничтожить сам объект через lifetime сек.
        Destroy(gameObject, lifetime);
    }

    private void OnDrawGizmosSelected()
    {
        // Визуализация радиуса в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
