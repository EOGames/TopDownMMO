using System;
using UnityEngine;


public class Player : MonoBehaviour
{
    public string playerId;
    public bool isLocalPlayer;
    public string characterName;

    private Agent agent;
    private void Awake()
    {
        agent = GetComponent<Agent>();
    }

    public void SetPlayerInfo(string _playerId,string _characterName)
    {
        playerId = _playerId;
        characterName = _characterName;
        if (playerId == GameManager.Instance.localPlayerId)
        {
            // means its local player
            isLocalPlayer = true;
        }
    }

    public Agent GetAgent()
    {
        return agent;
    }
}