using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerManager[] players = new PlayerManager[4];
    public event Action OnPlayerManagersModified;

    NetworkVariable<FixedString32Bytes> boardSceneName = new();
    NetworkVariable<byte> numTurns = new();
    NetworkVariable<byte> currentTurn = new();

    // TODO
    //NetworkDictionary<byte, byte> spaceOverrides;

    public readonly NetworkBools boardBools = new();
    public readonly NetworkNums boardNums = new();



    public PlayerManager Player(ulong clientId) {
        return players.FirstOrDefault(player => player.IsActive && player.ClientId == clientId);
    }

    public PlayerManager LocalPlayer => Player(NetworkManager.LocalClientId);


    public void AddPlayer(ulong clientId) {
        var player = players.First(player => !player.IsActive);
        player.ClientId = clientId;
        byte colorIndex;
        for (colorIndex = 0; colorIndex < 3; colorIndex++) {
            if (!players.Any(p => p != player && p.IsActive && p.ColorIndex == colorIndex)) {
                break;
            }
        }
        player.ColorIndex = colorIndex;
    }

    public void RemovePlayer(ulong clientId) {
        players.First(player => player.IsActive && player.ClientId == clientId).Deactivate();
    }

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        foreach (var player in players)
        {
            player.OnPlayerModified += () => OnPlayerManagersModified?.Invoke();
        }
    }

    public void StartGame(string sceneName) {
        boardSceneName.Value = sceneName;
        NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }


}
