﻿namespace Application.DTOs;

public class CriarContaRequest
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
