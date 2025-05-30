using UnityEngine;

public class FireTrail : MonoBehaviour
{
    [Tooltip("������� ������ ���� ����")]
    [SerializeField] private float duration = 3f;
    [Tooltip("���� � �������")]
    [SerializeField] private float damagePerSecond = 5f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    // �����������, ����� ����� Rigidbody2D ������ � �������
    private void OnTriggerStay2D(Collider2D other)
    {
        // �������� ������ �� ������ ����� ����� ����������
        if (other.TryGetComponent<PlayerBhvr>(out var player))
        {
            int dmg = Mathf.CeilToInt(damagePerSecond * Time.deltaTime);
            if (dmg > 0)
                player.TakeDamage(dmg);
        }
    }
}