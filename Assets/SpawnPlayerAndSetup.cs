using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnPlayerAndSetup : NetworkBehaviour{
    public GameObject Player;
    GameObject temp;
    GameObject[] Cameras;
    public override void OnStartLocalPlayer()
    {
        CmdSpawnPlayer();

    }

    [Command]
    void CmdSpawnPlayer(){
        temp = Instantiate(Player);
        NetworkServer.SpawnWithClientAuthority(temp,gameObject);

    }
    private void Update()
    {
        if (hasAuthority == false)
        {
            return;
        }
        temp.transform.GetChild(0).tag = "Untagged";
        Cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject LocalGameObject in Cameras)
        {
            LocalGameObject.GetComponent<Camera>().enabled = false;
        }
    }
}
