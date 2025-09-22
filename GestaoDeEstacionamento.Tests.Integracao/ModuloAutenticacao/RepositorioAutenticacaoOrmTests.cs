using GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Comands;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Handlers;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Testes.Integracao.ModuloAutenticacao
{
    [TestClass]
    [TestCategory("Integração - Autenticacao")]
    public sealed class RepositorioAutenticacaoOrmTests
    {
        private AppDbContext _context = null!;
        private UserManager<Usuario> _userManager = null!;
        private IRefreshTokenProvider _refreshTokenProvider = null!;
        private ITokenProvider _tokenProvider = null!;

        [TestInitialize]
        public void Inicializar()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var store = new UserStore<Usuario, Cargo, AppDbContext, Guid>(_context);
            var passwordHasher = new PasswordHasher<Usuario>();
            _userManager = new UserManager<Usuario>(
                store, null, passwordHasher, null, null, null, null, null, null
            );

            _refreshTokenProvider = new RefreshTokenProvider(_context);
            _tokenProvider = new TestTokenProvider(); 
        }

        [TestMethod]
        public async Task FluxoCompletoDeAutenticacao_Deve_Funcionar()
        {
            var registrarHandler = new RegistrarUsuarioCommandHandler(_userManager, _tokenProvider, _refreshTokenProvider);
            var registrarCommand = new RegistrarUsuarioCommand(
                NomeCompleto: "Teste Integracao",
                Email: "teste@teste.com",
                Senha: "Senha@123",
                ConfirmarSenha: "Senha@123"
            );

            var registrarResult = await registrarHandler.Handle(registrarCommand, CancellationToken.None);
            Assert.IsTrue(registrarResult.IsSuccess);
            Assert.IsNotNull(registrarResult.Value);
            Assert.IsFalse(string.IsNullOrEmpty(registrarResult.Value.AccessToken.Chave));
            Assert.IsFalse(string.IsNullOrEmpty(registrarResult.Value.RefreshToken));

            var usuarioCriado = await _userManager.FindByEmailAsync("teste@teste.com");
            Assert.IsNotNull(usuarioCriado);

            var usuarioAutenticacao = await _userManager.FindByEmailAsync("teste@teste.com");
            var passwordCheck = await _userManager.CheckPasswordAsync(usuarioAutenticacao!, "Senha@123");
            Assert.IsTrue(passwordCheck);

            var accessToken = _tokenProvider.GerarAccessToken(usuarioAutenticacao!);
            var refreshToken = _refreshTokenProvider.GerarRefreshToken(usuarioAutenticacao!);
            await _refreshTokenProvider.SalvarRefreshTokenAsync(refreshToken);

            Assert.IsNotNull(accessToken);
            Assert.IsFalse(string.IsNullOrEmpty(accessToken.Chave));
            Assert.IsFalse(string.IsNullOrEmpty(refreshToken.Token));

            var refreshHandler = new RefreshTokenCommandHandler(_refreshTokenProvider, _userManager, _tokenProvider);
            var refreshCommand = new RefreshTokenCommand(refreshToken.Token);
            var refreshResult = await refreshHandler.Handle(refreshCommand, CancellationToken.None);

            Assert.IsTrue(refreshResult.IsSuccess);
            Assert.IsNotNull(refreshResult.Value);
            Assert.IsFalse(string.IsNullOrEmpty(refreshResult.Value.Chave));

            await _refreshTokenProvider.InvalidarRefreshTokenAsync(refreshToken.Token);
            var tokenInvalidado = await _refreshTokenProvider.ObterRefreshTokenAsync(refreshToken.Token);
            Assert.IsNull(tokenInvalidado); 
        }
        private class TestTokenProvider : ITokenProvider
        {
            public AccessToken GerarAccessToken(Usuario usuario)
            {
                return new AccessToken("TOKEN_FAKE", DateTime.UtcNow.AddMinutes(15),
                    new UsuarioAutenticado(usuario.Id, usuario.FullName ?? "", usuario.Email ?? ""));
            }
        }
    }
}
