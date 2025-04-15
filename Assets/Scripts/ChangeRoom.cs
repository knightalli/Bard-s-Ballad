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

    void Start()
    {
        cam = Camera.main.GetComponent<Camera>();
        if (cameraController == null)
        {
            // Найти камеру по тегу, если ссылка не назначена
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera"); // Или используйте ваш тег
            if (mainCamera != null)
            {
                cameraController = mainCamera.GetComponent<CameraController>();
                if (cameraController == null)
                {
                    Debug.LogError("CameraController не найден на MainCamera!");
                }
            }
            else
            {
                Debug.LogError("MainCamera с тегом MainCamera не найдена!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.transform.position += playerChangePos;
        cam.transform.position += cameraChangePos;
        cameraController.AddLimits(limitChangeLeft, limitChangeRight, limitChangeBottom, limitChangeUpper);
    }
}
