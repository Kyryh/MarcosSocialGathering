using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public TMP_InputField nameField;

    public static string GetPlayerName() {
        string name = PlayerPrefs.GetString("playerName", Environment.UserName);
        return string.IsNullOrEmpty(name) ? "Player" : name;
    }
    void Awake()
    {
        nameField.text = GetPlayerName();
    }

    public void SetName(string newName) {
        if (!string.IsNullOrEmpty(newName) && !string.IsNullOrWhiteSpace(newName)) {
            PlayerPrefs.SetString("playerName", newName);
        } else {
            nameField.text = GetPlayerName();
        }
    }
}
