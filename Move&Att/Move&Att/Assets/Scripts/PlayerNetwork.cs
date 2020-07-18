using Photon.Pun;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Creating player");
        PhotonNetwork.Instantiate(Path.Combine("Resources", "Player"), new Vector3(5, 0, -14), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
