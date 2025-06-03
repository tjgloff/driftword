using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public new void SendMessage(string message)
    {
        Debug.Log("Chat: " + message);
    }
}
