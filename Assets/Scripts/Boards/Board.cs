using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Board : NetworkBehaviour
{

    public Player[] players = new Player[4];

    void Start()
    {
        
    }

    void StartGame() {
        StartGameClientRPC();
    }

    [Rpc(SendTo.ClientsAndHost)]
    void StartGameClientRPC() {
        NetworkManager.LocalClient.PlayerObject.transform.position = Vector3.zero;
    }

    void Update()
    {
        
    }

    private void OnGUI() {
        if (IsServer) {
            if (GUI.Button(new Rect(500, 500, 100, 50), "Start game")) {
                StartGame();
            }
        }
    }
}
