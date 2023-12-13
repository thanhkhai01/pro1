using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class PhotonLaucher : MonoBehaviour , INetworkRunnerCallbacks
{
    [SerializeField] NetworkRunner _runner;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform spawnLocation;
    [SerializeField] MyCameraController myCameraController;

    public Action OnPlayeJoin;
    public enum PhotonLaucherState
    {
        StartGame,

    }

    public PhotonLaucherState CurrentPhotonLaucherState;

    private void Awake()
    {
        _runner = GetComponent<NetworkRunner>();
        myCameraController = FindAnyObjectByType<MyCameraController>();
        if(_runner == null )
        {
            _runner = this.gameObject.AddComponent<NetworkRunner>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartNewSession();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewSession()
    {
        _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            
        });

        //UI_PhotonLaucher.instance.UpdateUI<PhotonLaucherState>(CurrentPhotonLaucherState);

        //UI_PhotonLaucher.UpdateUIByState<PhotonLaucher.PhotonLaucherState> updateUIByState = new UI_PhotonLaucher.UpdateUIByState<PhotonLaucherState>();
        /*updateUIByState.enumType = PhotonLaucherState.StartGame;
        updateUIByState.UpdateUI();*/

    }


    #region Photon Interface
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connect to Server");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(player != _runner.LocalPlayer) { return; }
        _runner.Spawn(playerPrefab,spawnLocation.position,Quaternion.identity,player);
        OnPlayeJoin?.Invoke();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    #endregion
}
