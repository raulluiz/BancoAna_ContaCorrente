using BancoAna_ContaCorrente.Domain.Entities;
using Dapper;
using Domain.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
    private readonly string _connectionString;

    public ContaCorrenteRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<bool> CPFExiste(string cpf)
    {
        using var connection = new SqliteConnection(_connectionString);
        var result = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM contacorrente WHERE cpf = @cpf",
            new { cpf });

        return result > 0;
    }

    public async Task<int> GerarNumeroConta()
    {
        var random = new Random();
        int numero;

        using var connection = new SqliteConnection(_connectionString);

        do
        {
            numero = random.Next(100000, 999999);
            var exists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM contacorrente WHERE numero = @numero", new { numero });
            if (exists == 0) break;
        }
        while (true);

        return numero;
    }

    public async Task AdicionarAsync(ContaCorrente conta)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.ExecuteAsync(
            @"INSERT INTO contacorrente (idcontacorrente, numero, nome, cpf, senha, salt, ativo)
              VALUES (@Id, @Numero, @Nome, @CPF, @SenhaHash, @Salt, @Ativo)",
            conta);
    }
}
