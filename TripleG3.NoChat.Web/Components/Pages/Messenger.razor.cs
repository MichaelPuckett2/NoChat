using Microsoft.AspNetCore.Components.Web;
using TripleG3.NoChat.Web.Model;

namespace TripleG3.NoChat.Web.Components.Pages;

public sealed partial class Messenger
{
    private bool hasJoined;
    public string Key { get; set; } = string.Empty;
    public string Handle { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public List<ChatMessage> Messages { get; set; } = [];
    public List<string> Users { get; set; } = [];

    private void OnMessageSent(ChatMessage chatMessage)
    {
        if (chatMessage.Key == Key)
        {
            Messages.Add(chatMessage);
            InvokeAsync(StateHasChanged);
        }
    }

    private void OnJoinedChat(ChatMessage chatMessage)
    {
        if (chatMessage.Key == Key)
        {
            Users.Clear();
            if (ChatManager.HandlesInRooms.TryGetValue(Key, out IList<string>? handles))
            {
                foreach (var handle in handles)
                {
                    Users.Add(handle);
                }
            }
            InvokeAsync(StateHasChanged);
        }
    }

    private void OnLeftChat(ChatMessage chatMessage)
    {
        if (chatMessage.Key == Key)
        {
            Users.Clear();
            if (ChatManager.HandlesInRooms.TryGetValue(Key, out IList<string>? handles))
            {
                foreach (var handle in handles)
                {
                    Users.Add(handle);
                }
            }
            InvokeAsync(StateHasChanged);
        }
    }

    private void SendMessage()
    {
        if (!hasJoined) return;
        if (string.IsNullOrWhiteSpace(Handle))
        {
            Handle = $"Anonymous:{Guid.NewGuid().ToString()[..4]}";
        }
        if (string.IsNullOrWhiteSpace(Message)) return;
        ChatManager.SendMessage(new ChatMessage(Key, Handle, Message));
        Message = string.Empty;
    }

    private void MessageKeyPressed(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            SendMessage();
        }
    }

    private void OnMessagesCleared(string key)
    {
        if (Key == key)
        {
            Messages.Clear();
            Key = string.Empty;
            InvokeAsync(StateHasChanged);
        }
    }

    private void JoinChat()
    {
        if (hasJoined && !string.IsNullOrEmpty(Handle)) return;
        hasJoined = true;
        ChatManager.MessageSent += OnMessageSent;
        ChatManager.MessagesCleared += OnMessagesCleared;
        ChatManager.JoinedChat += OnJoinedChat;
        ChatManager.LeftChat += OnLeftChat;
        ChatManager.JoinChat(new ChatMessage(Key, Handle, string.Empty));
    }

    private void LeaveChat()
    {
        try
        {
            if (!hasJoined) return;
            hasJoined = false;
            ChatManager.MessageSent -= OnMessageSent;
            ChatManager.MessagesCleared -= OnMessagesCleared;
            ChatManager.JoinedChat -= OnJoinedChat;
            ChatManager.LeftChat -= OnLeftChat;
            //ChatManager.ClearMessages(Key);
            ChatManager.LeaveChat(new ChatMessage(Key, Handle, string.Empty));
            Key = string.Empty;
            Handle = string.Empty;
            Message = string.Empty;
        }
        catch { }
    }

    public void Dispose()
    {
        LeaveChat();
    }
}