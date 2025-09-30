using System;
using SocketIOClient;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private SocketIOUnity socket;

    public void RegisterSocket(SocketIOUnity _socket)
    {
        socket = null;
        socket = _socket;
        
        socket.On("shootWeapon",(SocketIOResponse response) =>
        {
            try
            {
                print("Received Shoot Event !");
                string JsonString = response.GetValue().ToString();
                print( "JSON STRING FOR WEAPON EVENT "+JsonString + " And Response was "+response);
                WeaponInfo wpn_info = JsonUtility.FromJson<WeaponInfo>(JsonString);
                print($" Shoot Weapon Event Recieved for Player Id is : {wpn_info.id}");
                // applying info to target
                Player player = GameManager.Instance.FindPlayer(wpn_info.id);
                if (player != null && !player.isLocalPlayer)
                {
                    MainThreadDispatcher.Instance.Enqueue(() =>
                    {
                        player.GetGun().Shoot();
                    });
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        });
    }

    public void CreateShootEvent(string playerId,string _animationTriggerToPlay)
    {
        WeaponInfo data = new()
        {
            id = playerId,
            animationTriggerToPlay = _animationTriggerToPlay
        };
        socket.Emit("shootWeapon",JsonUtility.ToJson(data));
        print("Shoot Event Sent !");
    }
}

public class WeaponInfo
{
    public string id;
    public string animationTriggerToPlay;
}
