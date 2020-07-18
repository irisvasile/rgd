using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetworkControl : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField]
    public Transform camera;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.AutomaticallySyncScene = true;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom("TrialRoom", roomOptions, TypedLobby.Default); 
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room TrialRoom");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room TrialRoom");
        CreatePlayer();
    }
    public void CreatePlayer()
    {
        Debug.Log("Creating player");
        GameObject Created = PhotonNetwork.Instantiate("Player", new Vector3(5, 0, -14), Quaternion.identity);
        Transform T = Created.transform;
        camera.GetComponent<CameraController>().SetTarget(T);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
