using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AsistenteIA.Models;
using AsistenteIA.Services;

namespace AsistenteIA.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;
    private List<ConversationRecord> _allConversations = new();

    [ObservableProperty]
    private ObservableCollection<ConversationRecord> conversations = new();

    [ObservableProperty]
    private string searchQuery = string.Empty;

    [ObservableProperty]
    private ConversationRecord? selectedConversation;

    public HistoryViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    // Se ejecuta cada vez que se abre la página para cargar los datos de SQLite
    public async Task LoadHistoryAsync()
    {
        // Asumiendo que tu DatabaseService tiene un método GetAllAsync o similar
        // Adapta esta línea según cómo llamaste a tu método en la Fase 10
        _allConversations = await _databaseService.GetAllAsync();
        UpdateList(_allConversations);
    }

    [RelayCommand]
    private void Search()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            UpdateList(_allConversations);
        }
        else
        {
            var filtered = _allConversations
                .Where(c => c.Prompt.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
            UpdateList(filtered);
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(ConversationRecord record)
    {
        if (record == null) return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Eliminar",
            "¿Estás seguro de que deseas eliminar esta consulta?",
            "Sí", "No");

        if (confirm)
        {
            await _databaseService.DeleteAsync(record);
            _allConversations.Remove(record);
            Search(); // Actualiza la lista visualmente
        }
    }

    [RelayCommand]
    private async Task GoToDetailAsync()
    {
        if (SelectedConversation == null) return;

        // Aquí irás a la DetailPage en el futuro (Fase 9)
        // await Shell.Current.GoToAsync(nameof(DetailPage), new Dictionary<string, object> { { "Record", SelectedConversation } });

        SelectedConversation = null; // Quita la selección visual
    }

    private void UpdateList(List<ConversationRecord> items)
    {
        Conversations.Clear();
        foreach (var item in items)
        {
            Conversations.Add(item);
        }
    }
}