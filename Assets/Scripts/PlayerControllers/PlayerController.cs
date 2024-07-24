using Unity.Netcode;
using UnityEngine;

public abstract class PlayerController : NetworkBehaviour {
    protected virtual void Awake() {
        if (GameManager.Instance == null)
            return;
        int index = transform.GetSiblingIndex();
        GameManager.Instance.players[index].CurrentPlayerController = this;
    }
}
