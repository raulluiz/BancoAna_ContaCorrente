using BancoAna_ContaCorrente.Domain.Entities;

namespace Domain.Interfaces;

public interface IContaCorrenteRepository
{
    Task<bool> CPFExiste(string cpf);
    Task<int> GerarNumeroConta();
    Task AdicionarAsync(ContaCorrente conta);
}
