namespace Application.DTOs;

public class CriarContaCorrenteRequest
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
}
