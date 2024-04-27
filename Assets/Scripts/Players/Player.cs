using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    public bool canManuallyMove = true;
    protected Vector2 moveDirection = Vector2.zero;
    public float moveSpeed;
    protected Rigidbody rb;
    protected virtual void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        if (IsOwner && LobbyManager.Instance != null) {
            LobbyManager.Instance.CreateLobby();
    }
}

    void Update() {
        if (!IsOwner)
            return;
        if (canManuallyMove) {
            UpdateMovement();
            if (moveDirection != Vector2.zero)
                UpdateRotation();
        }
    }

    private void UpdateMovement() {
        rb.velocity = new Vector3(
            moveDirection.x * moveSpeed,
            rb.velocity.y,
            moveDirection.y * moveSpeed
        );
    }

    private void UpdateRotation() {
        transform.rotation = Quaternion.LookRotation(
            new Vector3(
                moveDirection.x,
                0,
                moveDirection.y
            ),
            transform.up
        );
    }
}
