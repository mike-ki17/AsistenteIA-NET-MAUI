using SQLite;
using AsistenteIA.Models;

namespace AsistenteIA.Services;
public class DatabaseService
{
    private SQLiteAsyncConnection? _database;

    private async Task InitAsync()
    {
        if (_database is not null)
            return;

        // Define la ruta segura en el almacenamiento local del dispositivo
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "asistente_ia.db3");
        _database = new SQLiteAsyncConnection(dbPath);

        // Crea la tabla basada en el modelo ConversationRecord
        await _database.CreateTableAsync<ConversationRecord>();
    }

    // Obtener todos los registros del historial
    public async Task<List<ConversationRecord>> GetConversationsAsync()
    {
        await InitAsync();
        return await _database!.Table<ConversationRecord>().ToListAsync();
    }

    // Guardar una nueva consulta y respuesta
    public async Task<int> SaveConversationAsync(ConversationRecord record)
    {
        await InitAsync();
        return await _database!.InsertAsync(record);
    }

    // Actualizar un registro (por ejemplo, al marcar como verificado)
    public async Task<int> UpdateConversationAsync(ConversationRecord record)
    {
        await InitAsync();
        return await _database!.UpdateAsync(record);
    }

    // Eliminar un registro del historial
    public async Task<int> DeleteConversationAsync(ConversationRecord record)
    {
        await InitAsync();
        return await _database!.DeleteAsync(record);
    }
}
