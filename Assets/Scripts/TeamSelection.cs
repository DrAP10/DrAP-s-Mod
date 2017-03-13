using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TeamSelection : MonoBehaviour {

    //GUI
    GUIStyle guiStyle;
    //PREFABS
    //public GameObject HunterPlayer;
    //public GameObject PropPlayer;
    // Use this for initialization
    void Start () {
        guiStyle = new GUIStyle();
        guiStyle.normal.textColor = Color.black;
        guiStyle.fontSize = 20;
        //NetworkServer.RegisterHandler(MsgType.AddPlayer, OnAddPlayerMessage);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
       // if (!gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
         //   return;
        if(GUI.Button(new Rect(0, 0, Screen.width/2, Screen.height), "Hunter", guiStyle))
        {
            /*GameObject player = Instantiate(HunterPlayer, transform.position, transform.rotation) as GameObject;
            NetworkServer.AddPlayerForConnection(conn ,player,0);
            Destroy(gameObject);*/
            CustomNetworkManager manager = gameObject.GetComponent<CustomNetworkManager>();
            manager.curPlayer = 0;
            manager.StartHost();
        }
        if (GUI.Button(new Rect(Screen.width / 2 + 1, 0, Screen.width / 2, Screen.height), "Prop", guiStyle))
        {
            /*GameObject player = Instantiate(PropPlayer, transform.position, transform.rotation) as GameObject;
            NetworkServer.Spawn(player);
            Destroy(gameObject);*/
            CustomNetworkManager manager = gameObject.GetComponent<CustomNetworkManager>();
            manager.curPlayer = 1;
            manager.StartHost();
        }
    }
    /*void OnAddPlayerMessage(NetworkMessage netMsg)
    {
        GameObject player = Instantiate(PropPlayer, transform.position, transform.rotation) as GameObject;
        // This spawns the new player on all clients
        NetworkServer.AddPlayerForConnection(netMsg.conn, player, 0);
    }*/
}
