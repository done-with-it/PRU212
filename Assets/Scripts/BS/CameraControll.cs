using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform player;

    // Offset giữa camera và player
    public Vector3 offset;

    // Tham số để tinh chỉnh tốc độ di chuyển của camera
    public float smoothSpeed = 0.125f;

    // Kích thước cửa sổ cố định
    public int screenWidth = 1280;
    public int screenHeight = 720;

    // Start is called before the first frame update
    void Start()
    {
        // Set kích thước cửa sổ cố định
        Screen.SetResolution(screenWidth, screenHeight, false);

        // Nếu không có player được gán, tìm player trong scene
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Di chuyển camera theo player
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
