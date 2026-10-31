using System.Net.Http.Json;
using AsistenteIA.Models;

namespace AsistenteIA.Services;

public class AiService
{
    private readonly HttpClient _client;
    private readonly ApiKeyService _keyService;

    public AiService(HttpClient client, ApiKeyService keyService)
    {
        _client = client;
        _keyService = keyService;
    }

    public async Task<string> AskAsync(string prompt, string model, CancellationToken token = default)
    {
        string cleanPrompt = prompt.Trim();
        if (cleanPrompt.Length < 5) throw new ArgumentException("La consulta es demasiado corta.");

        string? apiKey = await _keyService.GetAsync();
        if (string.IsNullOrWhiteSpace(apiKey)) throw new InvalidOperationException("Configure la clave de la API.");

        var requestBody = new ChatRequest
        {
            SystemInstruction = new SystemInstruction
            {
                Parts = [new ChatPart { Text = "Eres un asistente académico. Responde de forma clara, directa y muy breve. Limita tu respuesta a un máximo de un párrafo." }]
            },
            Contents = [
                new ChatContent { Role = "user", Parts = [new ChatPart { Text = cleanPrompt }] }
            ]
        };

        // Gemini requiere el modelo en la URL, no en el JSON
        using var request = new HttpRequestMessage(HttpMethod.Post, $"models/{model}:generateContent");
        request.Headers.Add("x-goog-api-key", apiKey);
        request.Content = JsonContent.Create(requestBody);

        using HttpResponseMessage response = await _client.SendAsync(request, token);
        response.EnsureSuccessStatusCode();

        ChatResponse? result = await response.Content.ReadFromJsonAsync<ChatResponse>(cancellationToken: token);
        string? answer = result?.Candidates.FirstOrDefault()?.Content.Parts.FirstOrDefault()?.Text;

        return string.IsNullOrWhiteSpace(answer) ? "El servicio no devolvió contenido." : answer.Trim();
    }
}