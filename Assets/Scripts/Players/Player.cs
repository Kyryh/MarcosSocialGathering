using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    public Vector2 moveDirection = Vector2.zero;
    public float moveSpeed;
    public Rigidbody rb;
    public PlayerControls controls;
    protected virtual void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if (!IsOwner)
            return;
        if (controls != null)
            controls.PlayerUpdate(this);
    }

}
