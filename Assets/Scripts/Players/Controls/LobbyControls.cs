using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyControls : PlayerControls
{
    public override void PlayerUpdate(Player player)
    {
        if (player.moveDirection != Vector2.zero)
            UpdateRotation(player);
    }

    private void UpdateMovement(Player player) {
        player.rb.velocity = new Vector3(
            player.moveDirection.x * player.moveSpeed,
            player.rb.velocity.y,
            player.moveDirection.y * player.moveSpeed
        );
    }

    private void UpdateRotation(Player player) {
        player.transform.rotation = Quaternion.LookRotation(
            new Vector3(
                player.moveDirection.x,
                0,
                player.moveDirection.y
            ),
            player.transform.up
        );
    }
}
