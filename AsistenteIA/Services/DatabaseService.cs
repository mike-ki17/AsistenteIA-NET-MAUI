using SQLite;
using AsistenteIA.Models;

namespace AsistenteIA.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _connection;

    // Configura la ruta segura en el dispositivo móvil/PC
    private readonly string _dbPath = Path.Combine(FileSystem.AppDataDirectory, "AsistenteIA.db3");

    private async Task InitAsync()
    {
        if (_connection != null) return;

        // Inicializa la conexión y crea la tabla si no existe
        _connection = new SQLiteAsyncConnection(_dbPath);
        await _connection.CreateTableAsync<ConversationRecord>();
    }

    public async Task<List<ConversationRecord>> GetAllAsync()
    {
        await InitAsync();
        // Retorna la lista requerida por el HistoryViewModel
        return await _connection!.Table<ConversationRecord>().OrderByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task<int> SaveAsync(ConversationRecord record)
    {
        await InitAsync();
        if (record.Id != 0)
            return await _connection!.UpdateAsync(record);
        else
            return await _connection!.InsertAsync(record);
    }

    public async Task<int> DeleteAsync(ConversationRecord record)
    {
        await InitAsync();
        return await _connection!.DeleteAsync(record);
    }
}