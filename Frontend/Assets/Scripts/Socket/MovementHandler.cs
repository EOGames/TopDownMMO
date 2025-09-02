using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    private SocketIOUnity mySocket;
    public void RegisterHandler(SocketIOUnity socket)
    {
        mySocket = socket;
        mySocket.On("movement",(resp =>
        {
            string jsonString = resp.GetValue().ToString();
            MovementData data  = JsonUtility.FromJson<MovementData>(jsonString);
            Player foundPlayer = GameManager.Instance.FindPlayer(data.id);
            if (foundPlayer != null && foundPlayer.playerId != GameManager.Instance.localPlayerId)
            {
                foundPlayer.GetAgent().MoveCloneToTarget(data.target);
                print("Applied MovementData");
            }

        } ));
    }

    public void SendMovementData(Vector3 dest,string userId)
    {
        var data = new MovementData()
        {
            target = dest,
            id = userId
        };
        
        mySocket.Emit("movement", JsonUtility.ToJson(data));
    }
}

public class MovementData
{
    public Vector3 target;
    public string id;
}