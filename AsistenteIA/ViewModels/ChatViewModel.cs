using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AsistenteIA.Services;

namespace AsistenteIA.ViewModels;
public partial class ChatViewModel : ObservableObject
{
    private readonly AiService _aiService;
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
    private string selectedModel = "openai/gpt-5.4-nano";

    public ChatViewModel(AiService aiService) => _aiService = aiService;

    [RelayCommand]
    private async Task AskAsync()
    {
        if (IsBusy) return;
        _cts = new CancellationTokenSource();
        IsBusy = true;
        StatusMessage = "Consultando el servicio...";

        try
        {
            Answer = await _aiService.AskAsync(
                Prompt, SelectedModel, _cts.Token);
            StatusMessage = "Respuesta recibida. Verifique la información.";
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
