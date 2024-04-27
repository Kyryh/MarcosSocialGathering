using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Player
{
    InputAction moveAction;
    protected override void Awake()
    {
        base.Awake();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        if (!IsOwner)
            return;

        moveAction.performed += ctx => {
            moveDirection = ctx.ReadValue<Vector2>();
        };

        if (LobbyManager.Instance != null)
            LobbyManager.Instance.CreateLobby();

    }

}
