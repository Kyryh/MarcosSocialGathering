using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager Instance { get; private set; }


    public BoardDef[] boards;
    private NetworkVariable<int> currentBoardIndex = new();
    private BoardDef CurrentBoard => boards[currentBoardIndex.Value];

    public Selectable[] serverOnlySelectables;

    public GameObject boardsSelection;
    public Transform playersTab;
    public Transform colorSelector;
    private Button[] colorButtons;

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
        };

        colorButtons = new Button[colorSelector.childCount];
        for (int i = 0; i < colorButtons.Length; i++) {
            colorButtons[i] = colorSelector.GetChild(i).GetComponent<Button>();
        }
    }

    public override void OnNetworkSpawn() {

        NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
        GameManager.Instance.OnPlayerManagersModified += OnPlayerManagersModified;


        currentBoardIndex.OnValueChanged += OnCurrentBoardIndexChanged;
        OnCurrentBoardIndexChanged(0, 0);

    }


    public override void OnNetworkDespawn() {
        NetworkManager.Singleton.OnConnectionEvent -= OnConnectionEvent;
        GameManager.Instance.OnPlayerManagersModified -= OnPlayerManagersModified;

        currentBoardIndex.OnValueChanged -= OnCurrentBoardIndexChanged;

    }

    private void OnPlayerManagersModified() {
        int index = 0;
        foreach (var player in GameManager.Instance.players) {
            if (player.IsActive) {
                var playerTab = playersTab.GetChild(index);
                playerTab.gameObject.SetActive(true);
                playerTab.GetComponentInChildren<TMP_Text>().text = player.PlayerName;
                index++;
            }
        }
        
        for (int i = index; i < playersTab.childCount; i++)
        {
            playersTab.GetChild(i).gameObject.SetActive(false);
        }

        foreach (var button in colorButtons)
        {
            button.interactable = true;
        }
        foreach (var player in GameManager.Instance.players) {
            if (player.IsActive) {
                colorButtons[player.ColorIndex].interactable = false;
            }
        }
    }

    private void OnCurrentBoardIndexChanged(int _, int __) {
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
        }
        if (networkManager.LocalClientId == eventData.ClientId) {
            switch (eventData.EventType) {
                case ConnectionEvent.ClientConnected:
                    CreateLobby();
                    OnCurrentBoardIndexChanged(0, 0);
                    break;
                case ConnectionEvent.ClientDisconnected:
                    NetworkManagerHelper.Instance.Shutdown();
                    break;
            }
        }
    }
    private void OnClientConnected(ulong clientId) {
        //OnCurrentBoardIndexChanged();
        GameManager.Instance.AddPlayer(clientId);
    }

    private void OnClientDisconnected(ulong clientId) {
        GameManager.Instance.RemovePlayer(clientId);
    }

    public void ChangeColor(int colorIndex) {
        ChangeColorServerRPC(NetworkManager.LocalClientId, colorIndex);
    }

    [Rpc(SendTo.Server)]
    void ChangeColorServerRPC(ulong clientId, int colorIndex) {
        GameManager.Instance.Player(clientId).ColorIndex = (byte)colorIndex;
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

    public void StartGame() {
        GameManager.Instance.StartGame(CurrentBoard.boardSceneName);
    }
}
