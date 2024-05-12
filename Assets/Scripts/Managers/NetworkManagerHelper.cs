using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagerHelper : MonoBehaviour
{
    public static NetworkManagerHelper Instance { get; private set; }
    private void Awake() {
        Instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartHost() {
        UnityTransport unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        unityTransport.SetConnectionData("127.0.0.1", (ushort)2626, "0.0.0.0");
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient(Transform canvas) {
        UnityTransport unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        Transform mainMenu = canvas.Find("Main Menu");

        string ip = mainMenu.Find("IP InputField").GetComponent<TMP_InputField>().text;
        unityTransport.SetConnectionData(ip, (ushort)2626);

        if (NetworkManager.Singleton.StartClient()) {
            Transform joiningMenu = canvas.Find("Joining Menu");

            mainMenu.gameObject.SetActive(false);
            joiningMenu.gameObject.SetActive(true);

            joiningMenu.Find("Joining Text").GetComponent<TMP_Text>().text = $"Joining lobby at ip {ip}\nPlease wait...";
        }
    }

    public void Shutdown() {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("MainMenu");
        foreach (var player in GameManager.Instance.players)
        {
            player.Deactivate();
        }
    }
}
