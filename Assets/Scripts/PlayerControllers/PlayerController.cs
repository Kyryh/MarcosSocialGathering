using Unity.Netcode;
using UnityEngine;

public abstract class PlayerController : NetworkBehaviour {
    protected virtual void Awake() {
        int index = transform.GetSiblingIndex();
        GameManager.Instance.players[index].CurrentPlayerController = this;
    }
}
