using UnityEngine;


public class Player : MonoBehaviour
{
    public string playerId;
    public bool isLocalPlayer;
    public string characterName;

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
}