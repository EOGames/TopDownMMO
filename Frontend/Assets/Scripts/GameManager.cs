using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    [SerializeField] private Player[] playerPrefabs;
    public string localPlayerId;
    
    [Header(("Settings to set at login time"))]
    [Space]
    public int selectedCharacter = 0;
    public Vector3 lastSpawnPosition = Vector3.zero;
    public string characterName;
    public Player spawned_localPlayer;
    private Dictionary<string, Player> spawnedPlayers = new Dictionary<string, Player>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Player FindPlayer(int id)
    {
        return playerPrefabs[id];
    }
    
    public void RegisterPlayer(string id, Player player)
    {
        spawnedPlayers[id] = player;
    }

    public void RemovePlayer(string id)
    {
        if (spawnedPlayers.TryGetValue(id, out Player player))
        {
            Destroy(player.gameObject);
            spawnedPlayers.Remove(id);
            Debug.Log("Removed player: " + id);
        }
    }

    public bool IsPlayerSpawned(string id) => spawnedPlayers.ContainsKey(id);

    public Player FindPlayer(string id)
    {
        return spawnedPlayers.GetValueOrDefault(id);
    }

}
