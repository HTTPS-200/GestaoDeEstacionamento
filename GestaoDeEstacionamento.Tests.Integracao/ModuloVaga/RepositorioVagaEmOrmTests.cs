using FizzWare.NBuilder;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Tests.Integracao.Compartilhado;

namespace GestaoDeEstacionamento.Tests.Integracao.ModuloVaga;

[TestClass]
[TestCategory("Integração - Vaga")]
public class RepositorioVagaEmOrmTests : TestFixture
{
    [TestMethod]
    public async Task Deve_Cadastrar_Vaga()
    {
        var vaga = new Vaga("V-101", "A", Guid.NewGuid());

        await repositorioVaga!.CadastrarAsync(vaga);
        await dbContext!.SaveChangesAsync();

        var vagaDb = await repositorioVaga.ObterPorIdentificador("V-101");
        Assert.IsNotNull(vagaDb);
        Assert.AreEqual("A", vagaDb!.Zona);
        Assert.IsFalse(vagaDb.Ocupada);
    }

    [TestMethod]
    public async Task Deve_Obter_Vaga_Por_Identificador()
    {
        var vaga = new Vaga("V-102", "B", Guid.NewGuid());
        await repositorioVaga!.CadastrarAsync(vaga);
        await dbContext!.SaveChangesAsync();

        var vagaDb = await repositorioVaga.ObterPorIdentificador("V-102");

        Assert.IsNotNull(vagaDb);
        Assert.AreEqual("B", vagaDb!.Zona);
    }

    [TestMethod]
    public async Task Deve_Obter_Vagas_Livres_E_Ocupadas()
    {
        var vagaLivre = new Vaga("V-103", "C", Guid.NewGuid());
        var vagaOcupada = new Vaga("V-104", "C", Guid.NewGuid());
        vagaOcupada.Ocupar(Guid.NewGuid());

        await repositorioVaga!.CadastrarAsync(vagaLivre);
        await repositorioVaga!.CadastrarAsync(vagaOcupada);
        await dbContext!.SaveChangesAsync();

        var livres = await repositorioVaga.ObterVagasLivres();
        var ocupadas = await repositorioVaga.ObterVagasOcupadas();

        Assert.IsTrue(livres.Any(v => v.Identificador == "V-103"));
        Assert.IsTrue(ocupadas.Any(v => v.Identificador == "V-104"));
    }

    [TestMethod]
    public async Task Deve_Ocupar_E_Liberar_Vaga()
    {
        var vaga = new Vaga("V-105", "D", Guid.NewGuid());
        await repositorioVaga!.CadastrarAsync(vaga);
        await dbContext!.SaveChangesAsync();

        // Ocupa
        var veiculoId = Guid.NewGuid();
        vaga.Ocupar(veiculoId);
        await repositorioVaga.EditarAsync(vaga.Id, vaga);
        await dbContext.SaveChangesAsync();

        var vagaDb = await repositorioVaga.ObterPorIdentificador("V-105");
        Assert.IsTrue(vagaDb!.Ocupada);
        Assert.AreEqual(veiculoId, vagaDb.VeiculoId);

        // Libera
        vagaDb.Liberar();
        await repositorioVaga.EditarAsync(vagaDb.Id, vagaDb);
        await dbContext.SaveChangesAsync();

        var vagaLiberada = await repositorioVaga.ObterPorIdentificador("V-105");
        Assert.IsFalse(vagaLiberada!.Ocupada);
        Assert.IsNull(vagaLiberada.VeiculoId);
    }

    [TestMethod]
    public async Task Deve_Verificar_Disponibilidade()
    {
        var vaga = new Vaga("V-106", "E", Guid.NewGuid());
        await repositorioVaga!.CadastrarAsync(vaga);
        await dbContext!.SaveChangesAsync();

        var disponivel = await repositorioVaga.VerificarDisponibilidade("V-106");
        Assert.IsTrue(disponivel);

        vaga.Ocupar(Guid.NewGuid());
        await repositorioVaga.EditarAsync(vaga.Id, vaga);
        await dbContext.SaveChangesAsync();

        disponivel = await repositorioVaga.VerificarDisponibilidade("V-106");
        Assert.IsFalse(disponivel);
    }
}
