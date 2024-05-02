using Specky7;

namespace TripleG3.NoChat.Web.Model;

[Singleton]
public class ChatManager
{
    public event Action<ChatMessage> MessageSent = delegate { };
    public event Action<ChatMessage> JoinedChat = delegate { };
    public event Action<ChatMessage> LeftChat = delegate { };
    public event Action<string> MessagesCleared = delegate { };
    public void SendMessage(ChatMessage chatMessage) => MessageSent(chatMessage);
    public void JoinChat(ChatMessage chatMessage)
    {
        if (HandlesInRooms.TryGetValue(chatMessage.Key, out IList<string>? handles))
        {
            handles.Add(chatMessage.Handle);
        }
        else
        {
            List<string> newHandles = [chatMessage.Handle];
            HandlesInRooms.Add(chatMessage.Key, newHandles);
        }
        JoinedChat(chatMessage);
    }
    public void LeaveChat(ChatMessage chatMessage)
    {
        if (HandlesInRooms.TryGetValue(chatMessage.Key, out IList<string>? handles))
        {
            handles.Remove(chatMessage.Handle);
        }
        LeftChat(chatMessage);
    }
    public void ClearMessages(string key) => MessagesCleared(key);
    public IDictionary<string, IList<string>> HandlesInRooms { get; } = new Dictionary<string, IList<string>>();
}
