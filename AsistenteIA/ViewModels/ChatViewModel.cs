using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AsistenteIA.Services;
using AsistenteIA.Models; // Importante para poder usar ConversationRecord

namespace AsistenteIA.ViewModels;

public partial class ChatViewModel : ObservableObject
{
    private readonly AiService _aiService;
    private readonly DatabaseService _databaseService; // 1. Declaramos el servicio de base de datos
    private CancellationTokenSource? _cts;

    [ObservableProperty]
    private string prompt = string.Empty;

    [ObservableProperty]
    private string answer = string.Empty;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    [ObservableProperty]    
    private bool isBusy;

    [ObservableProperty]
    private string selectedModel = "gemini-2.5-flash";

    // 2. Inyectamos DatabaseService en el constructor
    public ChatViewModel(AiService aiService, DatabaseService databaseService)
    {
        _aiService = aiService;
        _databaseService = databaseService;
    }

    [RelayCommand]
    private async Task AskAsync()
    {
        if (IsBusy) return;
        _cts = new CancellationTokenSource();
        IsBusy = true;
        StatusMessage = "Consultando el servicio...";

        try
        {
            Answer = await _aiService.AskAsync(Prompt, SelectedModel, _cts.Token);
            StatusMessage = "Respuesta recibida. Verifique la información.";

            // 3. CREAR Y GUARDAR EL REGISTRO EN SQLITE (Fase 10)
            var record = new ConversationRecord
            {
                Prompt = this.Prompt,
                Answer = this.Answer,
                Category = "General"
            };

            await _databaseService.SaveAsync(record);
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "Consulta cancelada o tiempo agotado.";
        }
        catch (HttpRequestException ex)
        {
            StatusMessage = $"Error del servicio: {ex.StatusCode}";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void Cancel() => _cts?.Cancel();
}