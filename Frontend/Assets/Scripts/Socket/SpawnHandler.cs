using System;
using UnityEngine;
using SocketIOClient;
public class SpawnHandler : MonoBehaviour
{
    private SocketIOUnity mySocket;
    public void RegisterHandler(SocketIOUnity socket)
    {
        mySocket = null;
        mySocket = socket;
        AfterSocketConnected();
    }


    void AfterSocketConnected()
    {
        //spawn player
        mySocket.On("spawnPlayer", (SocketIOResponse resp) =>
        {
            string playerInfo_Json = resp.GetValue().ToString();
            PlayerInfo info = JsonUtility.FromJson<PlayerInfo>(playerInfo_Json);
            //setting id here since this is the genrated id we received from server 1st time
            GameManager.Instance.localPlayerId = info.id;
            SpawnPlayer(info.id, info.spawnPosition, info.selectedCharacter, info.characterName);
        });
//spawn Exisiting Players
        mySocket.On("existingPlayers", (SocketIOResponse resp) =>
        {
            print("Exisiting Event");
            string playerInfo_Json = resp.GetValue().ToString();
            PlayerInfo info = JsonUtility.FromJson<PlayerInfo>(playerInfo_Json);
            print("Existing player list ::::: "+playerInfo_Json);
        });
        
        var requestPlayerSpawn = new PlayerInfo()
        {
            characterName = GameManager.Instance.characterName,
            spawnPosition = GameManager.Instance.lastSpawnPosition,
            selectedCharacter = GameManager.Instance.selectedCharacter,
        };
        print("<color=green>Player Spawn Request Sent !</color> " + requestPlayerSpawn);
        mySocket.Emit("requestPlayerSpawn", JsonUtility.ToJson(requestPlayerSpawn));
    }

    void SpawnPlayer(string id, Vector3 spawnPos, int selectedCharacter, string characterName)
    {
        if (!GameManager.Instance.IsPlayerSpawned(id))
        {
            print("Spawning Player ::: " + id + spawnPos + selectedCharacter + characterName);
            MainThreadDispatcher.Instance.Enqueue(() =>
            {
                print("Spawning now");
                Player tempPlayer = Instantiate(GameManager.Instance.FindPlayer(selectedCharacter));
                tempPlayer.transform.position = spawnPos;
                tempPlayer.SetPlayerInfo((id), characterName);
                GameManager.Instance.spawned_localPlayer = tempPlayer;
                GameManager.Instance.RegisterPlayer(id, tempPlayer);
            });
        }
        else
        {
            print($" Character id :{id} already spawned ");
        }
    }

    public void CallDisconnect()
    {
        
    }

    private void OnDestroy()
    {
        mySocket.Off("spawnPlayer");
    }
}
