using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CreatePlayer : NetworkBehaviour {
    
    public GameObject PropPlayer;
    public GameObject HunterPlayer;
    [SyncVar]
    public bool IsHunter;
    
    //public NetworkConnection conn;


    // Use this for initialization
    void Start () {
        CmdTransformar();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }
    [Command]
    void CmdTransformar()
    {
        Debug.Log("es hunter: " + IsHunter);
        NetworkConnection conn = connectionToClient;
        GameObject newPlayer;
        newPlayer = (IsHunter)?Instantiate<GameObject>(HunterPlayer):Instantiate<GameObject>(PropPlayer);
        Destroy(gameObject.gameObject);
        NetworkServer.Spawn(newPlayer);
        NetworkServer.ReplacePlayerForConnection(conn, newPlayer, 0);
        //NetworkServer.AddPlayerForConnection(conn, newPlayer, 0);
        //ClientScene.AddPlayer(conn, 0);
    }
}
