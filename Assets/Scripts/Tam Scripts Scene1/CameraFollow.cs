using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed = 0.1f;
    [SerializeField] private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance != null)
        {
            Vector3 targetPosition = PlayerController.Instance.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
        }
    }
}
