using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float lifetime = 1f;              // ����� ����� �������
    public float damage = 20f;               // ���� ����
    public float radius = 2f;                // ������ ������

    [Header("VFX")]
    public ParticleSystem particles;         // ������ ��� ��������� Particle System

    private void Start()
    {
        // ���� �� �������� ��������� ������� ����������:
        if (particles != null)
            particles.Play();

        // ���������� ��� ������ ����� lifetime ���.
        Destroy(gameObject, lifetime);
    }

    private void OnDrawGizmosSelected()
    {
        // ������������ ������� � ���������
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
