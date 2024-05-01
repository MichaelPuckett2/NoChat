using Specky7;

namespace TripleG3.NoChat.Web.Model;

[Singleton]
public class ChatManager
{
    public event Action<ChatMessage> MessageSent = delegate { };
    public event Action<string> MessagesCleared = delegate { };
    public void SendMessage(ChatMessage chatMessage) => MessageSent(chatMessage);
    public void ClearMessages(string key) => MessagesCleared(key);
}
