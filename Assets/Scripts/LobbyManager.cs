using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    public GameObject[] platforms = new GameObject[4];
    public static LobbyManager Instance { get; private set; }

    [HideInInspector]
    [DoNotSerialize]
    public ulong?[] platformsOccupations = new ulong?[4];

    public BoardDef[] boards;
    private NetworkVariable<int> currentBoardIndex = new();
    private BoardDef CurrentBoard => boards[currentBoardIndex.Value];

    public Selectable[] serverOnlySelectables;

    public GameObject boardsSelection;

    public UnityEvent OnCreateLobby;
    void Start()
    {
        Instance = this;

        var boardsDropdown = boardsSelection.GetComponentInChildren<TMP_Dropdown>();
        foreach (BoardDef board in boards) {
            boardsDropdown.options.Add(new TMP_Dropdown.OptionData(board.boardName));
        }
        boardsDropdown.RefreshShownValue();

        NetworkManager.Singleton.ConnectionApprovalCallback = (approvalRequest, approvalResponse) => {
            approvalResponse.Approved = NetworkManager.ConnectedClientsIds.Count < 4;
            approvalResponse.CreatePlayerObject = true;
            approvalResponse.Position = new Vector3(0, 10, 0);
            approvalResponse.Rotation = Quaternion.Euler(0, 180, 0);
        };
    }

    public override void OnNetworkSpawn() {

        NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;


        currentBoardIndex.OnValueChanged += (_, _) => OnCurrentBoardIndexChanged();
        OnCurrentBoardIndexChanged();

    }

    public override void OnNetworkDespawn() {
        NetworkManager.Singleton.OnConnectionEvent -= OnConnectionEvent;

    }

    private void OnCurrentBoardIndexChanged() {
        boardsSelection.GetComponentInChildren<TMP_Dropdown>().value = currentBoardIndex.Value;
        boardsSelection.transform.Find("Board Preview/Image").GetComponent<Image>().sprite = CurrentBoard.boardPreview;
        boardsSelection.transform.Find("Board Preview/Description").GetComponent<TMP_Text>().text = CurrentBoard.boardDescription;
    }

    private void OnConnectionEvent(NetworkManager networkManager, ConnectionEventData eventData) {
        if (networkManager.IsServer) {
            switch (eventData.EventType) {
                case ConnectionEvent.ClientConnected:
                    OnClientConnected(eventData.ClientId);
                    break;
                case ConnectionEvent.ClientDisconnected:
                    OnClientDisconnected(eventData.ClientId);
                    break;
            }
        } else {
            switch (eventData.EventType) {
                case ConnectionEvent.ClientConnected:
                    currentBoardIndex.OnValueChanged.Invoke(0, currentBoardIndex.Value);
                    break;
                case ConnectionEvent.ClientDisconnected:
                    NetworkManagerHelper.Instance.Shutdown();
                    break;
            }
        }
    }
    private void OnClientConnected(ulong clientId) {
        SetInitialPositionRPC(
            GetFreePlatform(clientId) ?? Vector3.zero,
            RpcTarget.Single(clientId, RpcTargetUse.Temp)
        );
        for (int i = 0; i < platforms.Length; i++) {
            PlatformSetActiveRPC(i, platforms[i].activeSelf);
        }
        currentBoardIndex.OnValueChanged.Invoke(0, 0);

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
        foreach (var selectable in serverOnlySelectables)
        {
            selectable.interactable = NetworkManager.IsServer;
        }
        OnCreateLobby?.Invoke();
    }

    public void OnBoardSelected(int index) {
        currentBoardIndex.Value = index;
    }

    private void Update() {

    }
}
