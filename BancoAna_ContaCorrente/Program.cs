using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Executa o script SQLite ao iniciar
EnsureDatabaseCreated(app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


// Função que cria o banco se não existir
void EnsureDatabaseCreated(IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    // Caminho relativo ao diretório de execução do app
    var sqlFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "Scripts", "contacorrente.sql");

    if (!File.Exists(sqlFilePath))
    {
        Console.WriteLine($"⚠️ Script SQL não encontrado: {sqlFilePath}");
        return;
    }

    var script = File.ReadAllText(sqlFilePath);

    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    using var command = connection.CreateCommand();
    command.CommandText = script;
    command.ExecuteNonQuery();

    Console.WriteLine("✅ Script contacorrente.sql executado com sucesso.");
}
