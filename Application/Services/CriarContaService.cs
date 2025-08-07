using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using BancoAna_ContaCorrente.Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class CriarContaService
{
    private readonly IContaCorrenteRepository _repo;

    public CriarContaService(IContaCorrenteRepository repo)
    {
        _repo = repo;
    }

    public async Task<(CriarContaResponse? resposta, string? erroTipo, string? erroMensagem)> CriarAsync(CriarContaRequest request)
    {
        if (!CpfEhValido(request.CPF))
            return (null, "INVALID_DOCUMENT", "CPF inválido.");

        if (await _repo.CPFExiste(request.CPF))
            return (null, "INVALID_DOCUMENT", "CPF já cadastrado.");

        var numeroConta = await _repo.GerarNumeroConta();
        var (hash, salt) = SenhaHasher.HashSenha(request.Senha);

        var conta = new ContaCorrente
        {
            Numero = numeroConta,
            Nome = request.Nome,
            CPF = request.CPF,
            SenhaHash = hash,
            Salt = salt,
            Ativo = true
        };

        await _repo.AdicionarAsync(conta);

        return (new CriarContaResponse { NumeroConta = numeroConta }, null, null);
    }

    private bool CpfEhValido(string cpf)
    {
        return cpf.Length == 11 && cpf.All(char.IsDigit); // Simples para exemplo
    }
}
