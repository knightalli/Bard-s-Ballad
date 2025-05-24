using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoom : MonoBehaviour
{
    public Vector3 cameraChangePos;
    public Vector3 playerChangePos;
    private Camera cam;
    public CameraController cameraController;
    public float limitChangeLeft;
    public float limitChangeRight;
    public float limitChangeBottom;
    public float limitChangeUpper;

    private Room currentRoom;
    private Room nextRoom;

    void Start()
    {
        cam = Camera.main.GetComponent<Camera>();
        if (cameraController == null)
        {
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            if (mainCamera != null)
            {
                cameraController = mainCamera.GetComponent<CameraController>();
            }
        }

        currentRoom = transform.parent.GetComponent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position += playerChangePos;
            cam.transform.position += cameraChangePos;
            cameraController.AddLimits(limitChangeLeft, limitChangeRight, limitChangeBottom, limitChangeUpper);

            Vector2 nextRoomPos = (Vector2)transform.position + (Vector2)playerChangePos;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(nextRoomPos, 0.1f);
            foreach (Collider2D collider in colliders)
            {
                Room room = collider.GetComponent<Room>();
                if (room != null && room != currentRoom)
                {
                    nextRoom = room;
                    break;
                }
            }

            if (nextRoom != null && !nextRoom.isVisited)
            {
                nextRoom.isVisited = true;
                nextRoom.OnRoomEnter();
            }
        }
    }
}
