using System.Text.Json.Serialization;

namespace AsistenteIA.Models;

public class ChatRequest
{
    [JsonPropertyName("contents")]
    public List<ChatContent> Contents { get; set; } = [];

    [JsonPropertyName("systemInstruction")]
    public SystemInstruction? SystemInstruction { get; set; }
}

public class SystemInstruction
{
    [JsonPropertyName("parts")]
    public List<ChatPart> Parts { get; set; } = [];
}

public class ChatContent
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("parts")]
    public List<ChatPart> Parts { get; set; } = [];
}

public class ChatPart
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}

public class ChatResponse
{
    [JsonPropertyName("candidates")]
    public List<ChatCandidate> Candidates { get; set; } = [];
}

public class ChatCandidate
{
    [JsonPropertyName("content")]
    public ChatContent Content { get; set; } = new();
}