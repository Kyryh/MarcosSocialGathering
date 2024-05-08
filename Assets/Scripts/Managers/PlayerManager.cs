using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    readonly Color[] colors = new Color[] {
        new(1.000f, 0.000f, 0.000f),
        new(0.000f, 0.219f, 1.000f),
        new(0.107f, 0.651f, 0.133f),
        new(1.000f, 0.993f, 0.000f),
        new(1.000f, 0.619f, 0.000f),
        new(1.000f, 0.495f, 0.952f),
        new(0.795f, 0.288f, 1.000f),
        new(0.443f, 0.257f, 0.098f),
        new(0.906f, 0.906f, 0.906f),
        new(0.151f, 0.151f, 0.151f),
        new(0.363f, 0.853f, 1.000f),
        new(0.425f, 1.000f, 0.000f),
        new(0.925f, 0.459f, 0.471f),
        new(1.000f, 0.991f, 0.656f),
        new(0.566f, 0.566f, 0.566f),
        new(0.882f, 0.776f, 0.600f)
    };

    NetworkVariable<sbyte> stars = new();
    NetworkVariable<ushort> coins = new();
    NetworkVariable<ulong> clientId = new();
    NetworkVariable<bool> active = new(false);
    NetworkVariable<FixedString32Bytes> playerName = new();
    NetworkVariable<byte> colorIndex = new();

    public bool IsActive => active.Value;

    public void Deactivate() => active.Value = false;

    public event Action OnPlayerModified;
    public ulong ClientId {
        get {
            return clientId.Value;
        }
        set {
            clientId.Value = value;
            active.Value = true;
            SetPlayerNameRPC(RpcTarget.Single(value, RpcTargetUse.Temp));
        }
    }

    public byte ColorIndex {
        get {
            return colorIndex.Value;
        }
        set {
            colorIndex.Value = value;
        }
    }

    public Color Color => colors[ColorIndex];
    public string PlayerName => IsActive ? playerName.Value.ToString() : "CPU";

    [Rpc(SendTo.SpecifiedInParams)]
    void SetPlayerNameRPC(RpcParams rpcParams) {
        SetPlayerNameServerRPC(OptionsManager.GetPlayerName());
    }

    [Rpc(SendTo.Server)]
    void SetPlayerNameServerRPC(string name) {
        playerName.Value = name;
    }

    private void Awake() {
        active.OnValueChanged += (_, _) => OnPlayerModified?.Invoke();
        clientId.OnValueChanged += (_, _) => OnPlayerModified?.Invoke();
        playerName.OnValueChanged += (_, _) => OnPlayerModified?.Invoke();

        colorIndex.OnValueChanged += (_, _) => OnPlayerModified?.Invoke();
    }
    public override string ToString() {
        return $"PlayerManager({ClientId}, {PlayerName}, {IsActive})";
    }
}
