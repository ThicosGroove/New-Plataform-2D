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

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector2 following = player.position;
        float clampX = Mathf.Clamp(following.x, minX, maxX);
        float clampY = Mathf.Clamp(following.y, minY, maxY);

        transform.position = Vector3.Lerp(transform.position, new Vector3(clampX, clampY, -10f) , smooth * Time.deltaTime);
        //transform.position = Vector3.Lerp(transform.position, new Vector3(0f, clampY, -10f) , smooth * Time.deltaTime);

    }
}
