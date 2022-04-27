using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Boundaries")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    [SerializeField] private float smooth;

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        float following = player.position.x;
        float clampX = Mathf.Clamp(following, minX, maxX);

        transform.position = Vector3.Lerp(transform.position, new Vector3(clampX, 0f, -10f) , smooth * Time.deltaTime);
    }
}
