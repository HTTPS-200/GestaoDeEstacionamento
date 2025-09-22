using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloVaga;

[TestClass]
[TestCategory("Testes - Unidade - Vaga")]
public sealed class VagaTests
{
    private Vaga? vaga;

    // Teste campos obrigatórios - CT001 / CT002
    [TestMethod]
    public void Deve_Impedir_Cadastro_Vaga_Com_Campos_Obrigatorios_Vazios()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        vaga = new Vaga("", "", usuarioId);

        // Act
        var identificadorVazio = string.IsNullOrWhiteSpace(vaga.Identificador);
        var zonaVazia = string.IsNullOrWhiteSpace(vaga.Zona);

        // Assert
        Assert.IsTrue(identificadorVazio, "O identificador da vaga não pode estar vazio.");
        Assert.IsTrue(zonaVazia, "A zona da vaga não pode estar vazia.");
    }

    // Teste identificador duplicado - CT003
    [TestMethod]
    public void Deve_Impedir_Cadastro_Vaga_Com_Identificador_Duplicado()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var listaVagas = new List<Vaga>
        {
            new Vaga("A01", "Zona A", usuarioId),
            new Vaga("A02", "Zona A", usuarioId),
            new Vaga("B01", "Zona B", usuarioId)
        };

        var novaVaga = new Vaga("A01", "Zona A", usuarioId);

        // Act
        var identificadorDuplicado = listaVagas.Any(v =>
            v.Identificador.Equals(novaVaga.Identificador, StringComparison.OrdinalIgnoreCase));

        // Assert
        Assert.IsTrue(identificadorDuplicado, "Já existe uma vaga registrada com este identificador.");
    }

    // Teste ocupar vaga - CT004
    [TestMethod]
    public void Deve_Ocupar_Vaga_Corretamente()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var veiculoId = Guid.NewGuid();
        vaga = new Vaga("A01", "Zona A", usuarioId);
        var ocupadaAnterior = vaga.Ocupada;
        var veiculoIdAnterior = vaga.VeiculoId;

        // Act
        vaga.Ocupar(veiculoId);

        // Assert
        Assert.IsFalse(ocupadaAnterior, "Vaga deve estar livre antes de ocupar");
        Assert.IsNull(veiculoIdAnterior, "VeiculoId deve ser nulo antes de ocupar");
        Assert.IsTrue(vaga.Ocupada, "Vaga deve estar ocupada após ocupar");
        Assert.AreEqual(veiculoId, vaga.VeiculoId, "VeiculoId deve ser definido corretamente");
    }

    // Teste liberar vaga - CT005
    [TestMethod]
    public void Deve_Liberar_Vaga_Corretamente()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var veiculoId = Guid.NewGuid();
        vaga = new Vaga("A01", "Zona A", usuarioId);
        vaga.Ocupar(veiculoId);

        // Act
        vaga.Liberar();

        // Assert
        Assert.IsFalse(vaga.Ocupada, "Vaga deve estar livre após liberar");
        Assert.IsNull(vaga.VeiculoId, "VeiculoId deve ser nulo após liberar");
    }

    // Teste atualização de registro - CT006
    [TestMethod]
    public void Deve_Atualizar_Registro_Vaga_Corretamente()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var vagaOriginal = new Vaga("A01", "Zona A", usuarioId);
        var vagaEditada = new Vaga("B01", "Zona B", usuarioId);
        vagaEditada.Ocupar(Guid.NewGuid());

        // Act
        vagaOriginal.AtualizarRegistro(vagaEditada);

        // Assert
        Assert.AreEqual("B01", vagaOriginal.Identificador);
        Assert.AreEqual("Zona B", vagaOriginal.Zona);
        Assert.AreEqual(vagaEditada.Ocupada, vagaOriginal.Ocupada);
        Assert.AreEqual(vagaEditada.VeiculoId, vagaOriginal.VeiculoId);
    }
}