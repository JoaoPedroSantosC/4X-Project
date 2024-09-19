using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Vector3 moveDir;
    void Update()
    {
        float verticalMovement = Input.GetAxisRaw("Vertical");
        float horizontalMovement = Input.GetAxisRaw("Horizontal");

        Vector3 camDir = Camera.main.transform.forward;

        moveDir = camDir;
        moveDir.y = 0;
        moveDir.Normalize();

        Vector3 verticalDir = moveDir * verticalMovement;

        Vector3 horizontalDir = Vector3.Cross(Vector3.up, moveDir) * horizontalMovement;

        moveDir = (verticalDir + horizontalDir).normalized;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
