using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    [SerializeField]
    private Traversable startingSpace;

    public Traversable StartingSpace => startingSpace;
    private void Awake() {
        Instance = this;
    }

    void OnDestroy() {
        Instance = null;
    }
    void Start() {
        if (GameManager.Instance == null)
            return;

        for (int i = 0; i < 4; i++)
        {
            GameManager.Instance.players[i].CurrentPlayerController.Rigidbody.MovePosition(
                StartingSpace.transform.position + new Vector3((i-2)*2, 0, -2)
            );
        }
    }

    void Update()
    {
        
    }

}
