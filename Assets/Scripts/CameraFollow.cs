using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Boundaries")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [SerializeField] private float smooth;

    GameObject player;

    private Camera cam;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
    }

    private void LateUpdate()
    {
         NormalCameraBehaviour();
    }

    private void NormalCameraBehaviour()
    {
        Vector2 following = player.transform.position;
        float clampX = Mathf.Clamp(following.x, minX, maxX);
        float clampY = Mathf.Clamp(following.y, minY, maxY);

        transform.position = Vector3.Lerp(transform.position, new Vector3(clampX, clampY, -10f), smooth * Time.deltaTime);
    }


    public void HighPositionCameraBehaviour()
    {
        cam.orthographicSize = 20f;
    }

}
