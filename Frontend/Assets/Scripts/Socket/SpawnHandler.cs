using System;
using UnityEngine;
using SocketIOClient;
using System.Collections.Generic;
using Newtonsoft.Json;
public class SpawnHandler : MonoBehaviour
{
    private SocketIOUnity mySocket;
    public void RegisterHandler(SocketIOUnity socket)
    {
        mySocket = null;
        mySocket = socket;
    }


   public void HandlePlayerSpawn(string userId)
    {
        //spawn player
        mySocket.On("spawnPlayer", (SocketIOResponse resp) =>
        {
            string playerInfo_Json = resp.GetValue().ToString();
            PlayerInfo info = JsonUtility.FromJson<PlayerInfo>(playerInfo_Json);
            //setting id here since this is the genrated id we received from server 1st time
            GameManager.Instance.localPlayerId = info.id;
            SpawnPlayer(info.id, info.spawnPosition, info.selectedCharacter, info.characterName,true);
        });
//spawn Exisiting Players
        mySocket.On("existingPlayers", (SocketIOResponse resp) =>
        {
            print("Exisiting Event");
            string playerInfo_Json = resp.GetValue().ToString();
            print($"Player info json before converting to array :{playerInfo_Json} ");
            string arrayJson = JsonHelper.DictionaryToArrayJson(playerInfo_Json);
            Dictionary<string, PlayerInfo> players = 
                JsonConvert.DeserializeObject<Dictionary<string, PlayerInfo>>(playerInfo_Json); 
            print("Existing player list ::::: After Convert to dictionary "+players);

            foreach (var p in players)
            {
                SpawnPlayer(p.Value.id, p.Value.spawnPosition, p.Value.selectedCharacter, p.Value.characterName, false);
            }
           
            
        });
        
        var requestPlayerSpawn = new PlayerInfo()
        {
            characterName = GameManager.Instance.characterName,
            spawnPosition = GameManager.Instance.lastSpawnPosition,
            selectedCharacter = GameManager.Instance.selectedCharacter,
            id = userId
        };
        print("<color=green>Player Spawn Request Sent !</color> " + requestPlayerSpawn);
        mySocket.Emit("requestPlayerSpawn", JsonUtility.ToJson(requestPlayerSpawn));
    }

    void SpawnPlayer(string id, Vector3 spawnPos, int selectedCharacter, string characterName,bool isLoginSpawn)
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
                if (isLoginSpawn) GameManager.Instance.spawned_localPlayer = tempPlayer;
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
