namespace BancoAna_ContaCorrente.Domain.Entities;

public class ContaCorrente
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int Numero { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
}
