using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerManager[] players = new PlayerManager[4];
    public event Action OnPlayerManagersModified;


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
        foreach (var player in players)
        {
            player.OnPlayerModified += () => OnPlayerManagersModified?.Invoke();
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    void Start()
    {
        
    }

    public void StartGame(string boardSceneName) {
        //boardState.Value.BoardSceneName = boardSceneName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
