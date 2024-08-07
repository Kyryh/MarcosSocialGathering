using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkDropdown : TMP_Dropdown {
    [Tooltip(
        "Should the selectable be interactable only by the server's owner?\n" +
        "If false, it will be interactable only to the GameManager::CurrentPlayer"
    )]
    [SerializeField]
    private bool serverOnly;
    public override bool IsInteractable() {
        return base.IsInteractable() && NetworkSelectable.CheckInteractable(serverOnly);
    }
}
