using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float dumping = 1.5f;
    public Vector2 offset = new Vector2(0f, 1f);
    public bool isDown;
    private Transform player;
    private int lastY;

    [SerializeField]
    private float initialLeftLimit; // Изначальные значения (SerializeField, чтобы задать в инспекторе)
    [SerializeField]
    private float initialRightLimit;
    [SerializeField]
    private float initialBottomLimit;
    [SerializeField]
    private float initialUpperLimit;


    public float leftLimit; // Текущие значения (public, чтобы другие скрипты не меняли напрямую, а через AddLimits)
    public float rightLimit;
    public float bottomLimit;
    public float upperLimit;

        public void AddLimits(float leftChange, float rightChange, float bottomChange, float upperChange)
    {
        leftLimit += leftChange;
        rightLimit += rightChange;
        bottomLimit += bottomChange;
        upperLimit += upperChange;
    }

    void Start()
    {
        offset = new Vector2(0f, Mathf.Abs(offset.y));
        FindPlayer(isDown);

        // Инициализация текущих границ из изначальных
        leftLimit = initialLeftLimit;
        rightLimit = initialRightLimit;
        bottomLimit = initialBottomLimit;
        upperLimit = initialUpperLimit;
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
        Gizmos.DrawLine(new Vector2(initialLeftLimit, initialUpperLimit), new Vector2(initialRightLimit, initialUpperLimit));
        Gizmos.DrawLine(new Vector2(initialLeftLimit, initialUpperLimit), new Vector2(initialLeftLimit, initialBottomLimit));
        Gizmos.DrawLine(new Vector2(initialLeftLimit, initialBottomLimit), new Vector2(initialRightLimit, initialBottomLimit));
        Gizmos.DrawLine(new Vector2(initialRightLimit, initialUpperLimit), new Vector2(initialRightLimit, initialBottomLimit));
    }
}
