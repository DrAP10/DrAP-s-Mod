using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    public GameObject PropPlayer;
    public GameObject HunterPlayer;

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        //Destroy(gamePlayer);
        //gamePlayer= Instantiate(PropPlayer) as GameObject;

        /*var conn = gamePlayer.GetComponent<NetworkBehaviour>().connectionToClient;
        var newPlayer = Instantiate<GameObject>(PropPlayer);
        newPlayer.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
        Destroy(gamePlayer.gameObject);

        NetworkServer.ReplacePlayerForConnection(conn, newPlayer, 0);*/
        //gamePlayer.GetComponent<CreatePlayer>().conn = lobbyPlayer.GetComponent<LobbyPlayer>().connectionToClient;
        CreatePlayer player = gamePlayer.GetComponent<CreatePlayer>();
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        
        player.IsHunter = lobby.playerColor.Equals(Color.red);
    }
}
