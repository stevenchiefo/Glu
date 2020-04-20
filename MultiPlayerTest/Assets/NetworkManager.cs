using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(this);

        NetworkConfig.InitNetwork();
        NetworkConfig.ConnectToServer();
    }

    private void Update()
    {
        if (NetworkConfig.m_Socket.IsConnected == false)
        {
            NetworkConfig.ConnectToServer();
        }
    }

    private void OnApplicationQuit()
    {
        NetworkConfig.DisconnectFromServer();
    }
}