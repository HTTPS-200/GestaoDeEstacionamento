using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
public class Veiculo : EntidadeBase<Veiculo>
{
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Cor { get; set; }

    public Veiculo(string placa, string modelo, string cor)
    {
        Placa = placa;
        Modelo = modelo;
        Cor = cor;
    }

    public override void AtualizarRegistro(Veiculo registro)
    {
        if (registro is Veiculo atualizacao)
        {
            Modelo = string.IsNullOrWhiteSpace(atualizacao.Modelo) ? Modelo : atualizacao.Modelo;
            Cor = string.IsNullOrWhiteSpace(atualizacao.Cor) ? Cor : atualizacao.Cor;
        }
    }
}

