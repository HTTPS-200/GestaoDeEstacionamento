using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using System.Diagnostics.CodeAnalysis;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;

public class Veiculo : EntidadeBase<Veiculo>
{
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Cor { get; set; }
    public string CPFHospede { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataEntrada { get; set; }
    public DateTime? DataSaida { get; set; }

    [ExcludeFromCodeCoverage]
    public Veiculo() { }

    public Veiculo(string placa, string modelo, string cor, string cpfHospede, string? observacoes = null)
    {
        Id = Guid.NewGuid();
        Placa = placa;
        Modelo = modelo;
        Cor = cor;
        CPFHospede = cpfHospede;
        Observacoes = observacoes;
        DataEntrada = DateTime.Now;
    }

    public override void AtualizarRegistro(Veiculo registroEditado)
    {
        Placa = registroEditado.Placa;
        Modelo = registroEditado.Modelo;
        Cor = registroEditado.Cor;
        CPFHospede = registroEditado.CPFHospede;
        Observacoes = registroEditado.Observacoes;
    }

    public void RegistrarSaida()
    {
        DataSaida = DateTime.Now;
    }
}