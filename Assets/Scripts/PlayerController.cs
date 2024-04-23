using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    InputAction moveAction;
    Rigidbody rb;
    public float moveSpeed;
    void Start()
    {
        if (!IsOwner)
            return;
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.performed += ctx => {
            Vector2 direction = ctx.ReadValue<Vector2>();
            if (direction == Vector2.zero)
                return;
            transform.rotation = Quaternion.LookRotation(
                new Vector3(
                    direction.x,
                    0,
                    direction.y
                ),
                transform.up
            );
        };
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
            return;
        UpdateMovement(moveAction.ReadValue<Vector2>());
    }

    private void UpdateMovement(Vector2 direction) {
        rb.velocity = new Vector3(
            direction.x * moveSpeed,
            rb.velocity.y,
            direction.y * moveSpeed
        );
    }
}
