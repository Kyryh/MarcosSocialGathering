using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    NetworkVariable<PlayerState> player1State = new();
    NetworkVariable<PlayerState> player2State = new();
    NetworkVariable<PlayerState> player3State = new();
    NetworkVariable<PlayerState> player4State = new();

    NetworkVariable<BoardState> boardState = new();

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    void Start()
    {
        
    }

    public void StartGame() {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    struct PlayerState : INetworkSerializable {
        public int stars;
        public int coins;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref stars);
            serializer.SerializeValue(ref coins);
        }
    }

    struct BoardState : INetworkSerializable {
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {

        }
    }

}
