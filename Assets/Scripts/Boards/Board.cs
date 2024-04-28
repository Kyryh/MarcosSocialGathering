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
    void Start()
    {
        
    }

    void Update()
    {
        
    }

}
