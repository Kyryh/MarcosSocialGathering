using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

public class LobbyManager : NetworkBehaviour
{
    public GameObject[] platforms = new GameObject[4];
    public static LobbyManager Instance { get; private set; }

    public ulong?[] platformsOccupations = new ulong?[4];
    

    public UnityEvent OnCreateLobby;
    void Start()
    {
        Instance = this;
        NetworkManager.Singleton.ConnectionApprovalCallback = (approvalRequest, approvalResponse) => {
            approvalResponse.Approved = NetworkManager.ConnectedClientsIds.Count < 4;
            approvalResponse.CreatePlayerObject = true;
            approvalResponse.Position = new Vector3 (0, 10, 0);
            approvalResponse.Rotation = Quaternion.Euler(0, 180, 0);
            
        };
        NetworkManager.Singleton.OnConnectionEvent += (networkManager, eventData) => {
            if (!networkManager.IsServer)
                return;
            switch (eventData.EventType) {
                case ConnectionEvent.ClientConnected:
                    OnClientConnected(eventData.ClientId);
                    break;
                case ConnectionEvent.ClientDisconnected:
                    OnClientDisconnected(eventData.ClientId);
                    break;
            }
        };
    }

    private void OnClientConnected(ulong clientId) {
        SetInitialPositionRPC(
            GetFreePlatform(clientId) ?? Vector3.zero,
            RpcTarget.Single(clientId, RpcTargetUse.Temp)
        );
        for (int i = 0; i < platforms.Length; i++) {
            PlatformSetActiveRPC(i, platforms[i].activeSelf);
        }
    }

    private void OnClientDisconnected(ulong clientId) {
        for (int i = 0; i < platformsOccupations.Length; i++) {
            if (platformsOccupations[i] == clientId) {
                PlatformSetActiveRPC(i, false);
                platformsOccupations[i] = null;
            }
        }
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void SetInitialPositionRPC(Vector3 position, RpcParams rpcParams) {
        NetworkManager.LocalClient.PlayerObject.transform.position = position;
    }
    private Vector3? GetFreePlatform(ulong clientId) {
        for (int i = 0; i < platforms.Length; i++)
        {
            GameObject platform = platforms[i];
            if (platformsOccupations[i] == null) {
                PlatformSetActiveRPC(i, true);
                platformsOccupations[i] = clientId;
                return platform.transform.position;
            }
        }
        return null;
    }

    [Rpc(SendTo.Everyone)]
    private void PlatformSetActiveRPC(int platformIndex, bool value) {
        platforms[platformIndex].SetActive(value);
    }

    public void CreateLobby() {
        OnCreateLobby?.Invoke();
    }
}
