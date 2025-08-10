using Application.DTOs;
using BancoAna_ContaCorrente.Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class CriarContaCorrenteService
{
    private readonly IContaCorrenteRepository _repo;

    public CriarContaCorrenteService(IContaCorrenteRepository repo)
    {
        _repo = repo;
    }

    public async Task<(CriarContaResponse? resposta, string? erroTipo, string? erroMensagem)> CriarContaAsync(CriarContaCorrenteRequest request)
    {
        if (!CpfEhValido(request.CPF))
            return (null, "INVALID_DOCUMENT", "CPF inválido.");

        if (await _repo.CPFExiste(request.CPF))
            return (null, "INVALID_DOCUMENT", "CPF já cadastrado.");

        var numeroConta = await _repo.GerarNumeroConta();
        var (hash, salt) = SenhaHasher.HashSenha(request.Senha);

        var conta = new ContaCorrente(request.Nome, hash, salt, numeroConta, request.CPF);

        await _repo.AdicionarAsync(conta);

        return (new CriarContaResponse { NumeroConta = numeroConta }, null, null);
    }

    private bool CpfEhValido(string cpf)
    {
        return cpf.Length == 11 && cpf.All(char.IsDigit); // Simples para exemplo
    }
}
