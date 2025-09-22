using DotNet.Testcontainers.Containers;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloCheckIn;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVaga;
using Testcontainers.PostgreSql;
using FizzWare.NBuilder;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVeiculo;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFatura;
using GestaoDeEstacionamento.Core.Dominio.ModuloFatura;
using GestaoDeEstacionamento.Core.Dominio.ModuloCheckIn;

namespace GestaoDeEstacionamento.Tests.Integracao.Compartilhado
{
    [TestClass]
    public abstract class TestFixture
    {
        protected AppDbContext? dbContext;
        protected RepositorioRegistroCheckInEmOrm? repositorioCheckIn;
        protected RepositorioFaturaEmOrm? repositorioFatura;
        protected RepositorioVagaEmOrm? repositorioVaga;
        protected RepositorioVeiculoEmOrm? repositorioVeiculo;
        protected RepositorioTicketEmOrm? repositorioTicket;

        private static IDatabaseContainer? dbContainer;

        [AssemblyInitialize]
        public static async Task Setup(TestContext _)
        {
            dbContainer = new PostgreSqlBuilder()
                .WithImage("postgres:16")
                .WithName("gestaoDeEstacionamento-testdb")
                .WithDatabase("GestaoDeEstacionamento")
                .WithUsername("postgres")
                .WithPassword("aSenhaAindaEfraca")
                .WithCleanUp(true)
                .Build();

            await InicializarBancoDadosAsync(dbContainer);
        }

        [AssemblyCleanup]
        public static async Task Teardown()
        {
            await EncerrarBancoDadosAsync();
        }

        [TestInitialize]
        public void ConfigurarTestes()
        {
            if (dbContainer == null)
                throw new ArgumentException("O Banco de Dados não foi inicializado.");

            dbContext = AppDbContextFactory.CriarDbContext(dbContainer.GetConnectionString());

            ConfigurarTabelas(dbContext);

            repositorioCheckIn = new RepositorioRegistroCheckInEmOrm(dbContext);
            repositorioFatura = new RepositorioFaturaEmOrm(dbContext);
            repositorioVaga = new RepositorioVagaEmOrm(dbContext);
            repositorioVeiculo = new RepositorioVeiculoEmOrm(dbContext);
            repositorioTicket = new RepositorioTicketEmOrm(dbContext);

            BuilderSetup.SetCreatePersistenceMethod<Fatura>(async x => await repositorioFatura.CadastrarAsync(x));
            BuilderSetup.SetCreatePersistenceMethod<IList<Fatura>>(async x => await repositorioFatura.CadastrarEntidades(x));

            BuilderSetup.SetCreatePersistenceMethod<RegistroCheckIn>(async x => await repositorioCheckIn.CadastrarAsync(x));
            BuilderSetup.SetCreatePersistenceMethod<IList<RegistroCheckIn>>(async x => await repositorioCheckIn.CadastrarEntidades(x));

            BuilderSetup.SetCreatePersistenceMethod<Vaga>(async x => await repositorioVaga.CadastrarAsync(x));
            BuilderSetup.SetCreatePersistenceMethod<IList<Vaga>>(async x => await repositorioVaga.CadastrarEntidades(x));

            BuilderSetup.SetCreatePersistenceMethod<Veiculo>(async x => await repositorioVeiculo.CadastrarAsync(x));
            BuilderSetup.SetCreatePersistenceMethod<IList<Veiculo>>(async x => await repositorioVeiculo.CadastrarEntidades(x));

            BuilderSetup.SetCreatePersistenceMethod<Ticket>(async x => await repositorioTicket.CadastrarAsync(x));
            BuilderSetup.SetCreatePersistenceMethod<IList<Ticket>>(async x => await repositorioTicket.CadastrarEntidades(x));

        }

        private static void ConfigurarTabelas(AppDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            dbContext.Fatura.RemoveRange(dbContext.Fatura);
            dbContext.Tickets.RemoveRange(dbContext.Tickets);
            dbContext.RegistrosCheckIn.RemoveRange(dbContext.RegistrosCheckIn);
            dbContext.Vaga.RemoveRange(dbContext.Vaga);
            dbContext.Veiculos.RemoveRange(dbContext.Veiculos);

            dbContext.SaveChanges();
        }

        private static async Task InicializarBancoDadosAsync(IDatabaseContainer dbContainer)
        {
            await dbContainer.StartAsync();
        }

        private static async Task EncerrarBancoDadosAsync()
        {
            if (dbContainer is null)
                throw new ArgumentNullException("O Banco de dados não foi inicializado.");

            await dbContainer.StopAsync();
            await dbContainer.DisposeAsync();
        }
    }
}