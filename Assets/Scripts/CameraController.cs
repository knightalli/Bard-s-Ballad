using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float dumping = 1.5f;
    public Vector2 offset = new Vector2(0f, 1f);
    public bool isDown;
    private Transform player;
    private int lastY;

    [SerializeField]
    float leftLimit;
    [SerializeField]
    float rightLimit;
    [SerializeField]
    float bottomLimit;
    [SerializeField]
    float upperLimit;

    void Start()
    {
        offset = new Vector2(0f, Mathf.Abs(offset.y));
        FindPlayer(isDown);
    }

    public void FindPlayer(bool playerIsDown)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastY = Mathf.RoundToInt(player.position.y);
        if (playerIsDown)
        {
            transform.position = new Vector3(0f, player.position.y - offset.y);
        }
        else
        {
            transform.position = new Vector3(0f, player.position.y + offset.y);
        }
    }

    void Update()
    {
        if (player)
        {
            int currentY = Mathf.RoundToInt(player.position.y);
            if (currentY > lastY) isDown = false;
            else if (currentY < lastY) isDown = true;
            lastY = Mathf.RoundToInt(player.position.y);

            Vector2 target;
            if (isDown)
            {
                target = new Vector3(0f, player.position.y - offset.y);
            }
            else
            {
                target = new Vector3(0f, player.position.y + offset.y);
            }

            Vector3 currentPosition = Vector3.Lerp(transform.position, target, dumping * Time.deltaTime);
            transform.position = currentPosition;
        }

        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
            Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
            transform.position.z
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(rightLimit, upperLimit));
        Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(leftLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(rightLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(rightLimit, upperLimit), new Vector2(rightLimit, bottomLimit));
    }
}
