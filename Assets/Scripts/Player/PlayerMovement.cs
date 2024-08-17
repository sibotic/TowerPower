using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed = 12;
    public float groundDrag = 2;
    public float sprintSpeedMultiplier = 2;

    Rigidbody rb;
    Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate(){
        MovePlayer();
    }

    void MovePlayer(){
        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
    }
}
