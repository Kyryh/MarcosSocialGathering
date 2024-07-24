using Unity.Netcode;
using UnityEngine;

public abstract class PlayerController : NetworkBehaviour {

    public Rigidbody Rigidbody { get; private set; }

    protected virtual void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
        if (GameManager.Instance == null)
            return;
        int index = transform.GetSiblingIndex();
        GameManager.Instance.players[index].CurrentPlayerController = this;
    }
}
