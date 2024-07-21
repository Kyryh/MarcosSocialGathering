using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkSelectable : Selectable {

    [Tooltip(
        "Should the selectable be interactable only by the server's owner?\n" +
        "If false, it will be interactable only to the GameManager::CurrentPlayer"
    )]
    [SerializeField]
    private bool serverOnly;
    public override bool IsInteractable() {
        return base.IsInteractable() && CheckInteractable(serverOnly);
    }

    public static bool CheckInteractable(bool serverOnly) {
        if (serverOnly)
            return NetworkManager.Singleton.IsServer;

        if (GameManager.Instance.CurrentPlayer == null)
            return true;
        return GameManager.Instance.CurrentPlayer == NetworkManager.Singleton.LocalClientId;
    }
}

