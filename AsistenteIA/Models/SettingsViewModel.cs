using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AsistenteIA.Services;
using System.Collections.ObjectModel;

namespace AsistenteIA.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly ApiKeyService _apiKeyService;
    private readonly AiService _aiService;

    [ObservableProperty]
    private string apiKey = string.Empty;

    [ObservableProperty]
    private string selectedModel = string.Empty;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    public ObservableCollection<string> AvailableModels { get; } = new()
    {
        "gemini-2.5-flash",
        "gemini-3.1-pro"
    };

    public SettingsViewModel(ApiKeyService apiKeyService, AiService aiService)
    {
        _apiKeyService = apiKeyService;
        _aiService = aiService;
        SelectedModel = AvailableModels.First();
        LoadKeyAsync();
    }

    private async void LoadKeyAsync()
    {
        var key = await _apiKeyService.GetAsync();
        if (!string.IsNullOrEmpty(key))
        {
            ApiKey = key;
        }
    }

    [RelayCommand]
    private async Task SaveKeyAsync()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            StatusMessage = "La clave no puede estar vacía.";
            return;
        }
        await _apiKeyService.SaveAsync(ApiKey);
        StatusMessage = "Clave guardada exitosamente.";
    }

    [RelayCommand]
    private void DeleteKey()
    {
        _apiKeyService.Remove();
        ApiKey = string.Empty;
        StatusMessage = "Clave eliminada.";
    }

    [RelayCommand]
    private async Task TestConnectionAsync()
    {
        StatusMessage = "Probando conexión...";
        try
        {
            // Realiza una consulta breve como exige la Fase 11
            var response = await _aiService.AskAsync("Di solo 'Hola'", SelectedModel);
            StatusMessage = $"¡Conexión exitosa! Respuesta: {response}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error de conexión: {ex.Message}";
        }
    }
}