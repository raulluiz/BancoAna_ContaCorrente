using Microsoft.Data.Sqlite;

namespace Infrastructure.Database;

public static class DatabaseInitializer
{
    public static void Initialize()
    {
        string sqlPath = Path.Combine(AppContext.BaseDirectory, "Data", "Scripts", "contacorrente.sql");
        string dbPath = Path.Combine(AppContext.BaseDirectory, "Data", "contacorrente.db");

        // Cria pasta Data se não existir
        Directory.CreateDirectory("Data");

        // Se o banco já existe, não recria
        if (File.Exists(dbPath))
            return;

        // Cria e conecta no banco
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        // Lê e executa o script SQL
        string script = File.ReadAllText(sqlPath);
        using var command = connection.CreateCommand();
        command.CommandText = script;
        command.ExecuteNonQuery();
    }
}
