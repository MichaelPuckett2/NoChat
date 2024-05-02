namespace TripleG3.NoChat.Web.Model;

public record ChatMessage(string Key, string Handle, string Message)
{
    public static ChatMessage Empty { get; } = new(string.Empty, string.Empty, string.Empty);
}