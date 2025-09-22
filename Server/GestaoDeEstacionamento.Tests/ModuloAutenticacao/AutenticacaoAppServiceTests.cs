using GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Comands;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Handlers;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentResults;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.TestsUnitarios.ModuloAutenticacao
{
    [TestClass]
    [TestCategory("Testes - Unidade/Aplicação - Autenticacao")]
    public sealed class AutenticacaoAppServiceTests
    {
        private Mock<UserManager<Usuario>>? userManagerMock;
        private Mock<SignInManager<Usuario>>? signInManagerMock;
        private Mock<ITokenProvider>? tokenProviderMock;
        private Mock<IRefreshTokenProvider>? refreshTokenProviderMock;

        private AutenticarUsuarioCommandHandler? autenticarHandler;
        private RegistrarUsuarioCommandHandler? registrarHandler;
        private RefreshTokenCommandHandler? refreshHandler;

        [TestInitialize]
        public void Setup()
        {
            var storeMock = new Mock<IUserStore<Usuario>>();
            userManagerMock = new Mock<UserManager<Usuario>>(storeMock.Object, null, null, null, null, null, null, null, null);
            signInManagerMock = new Mock<SignInManager<Usuario>>(userManagerMock.Object,
                new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<Usuario>>().Object, null, null, null, null);
            tokenProviderMock = new Mock<ITokenProvider>();
            refreshTokenProviderMock = new Mock<IRefreshTokenProvider>();

            autenticarHandler = new AutenticarUsuarioCommandHandler(signInManagerMock.Object, userManagerMock.Object, tokenProviderMock.Object, refreshTokenProviderMock.Object);
            registrarHandler = new RegistrarUsuarioCommandHandler(userManagerMock.Object, tokenProviderMock.Object, refreshTokenProviderMock.Object);
            refreshHandler = new RefreshTokenCommandHandler(refreshTokenProviderMock.Object, userManagerMock.Object, tokenProviderMock.Object);
        }

        [TestMethod]
        public async Task RegistrarUsuario_Deve_Retornar_AccessETokenERefreshToken()
        {
            // Arrange
            var usuario = new Usuario { UserName = "teste@teste.com", Email = "teste@teste.com", FullName = "Teste" };
            userManagerMock!.Setup(x => x.CreateAsync(It.IsAny<Usuario>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            tokenProviderMock!.Setup(x => x.GerarAccessToken(It.IsAny<Usuario>()))
                .Returns(new AccessToken("ACCESSTOKEN", DateTime.UtcNow.AddMinutes(15), new UsuarioAutenticado(usuario.Id, usuario.FullName, usuario.Email)));

            refreshTokenProviderMock!.Setup(x => x.GerarRefreshToken(It.IsAny<Usuario>()))
                .Returns(new RefreshToken { Token = "REFRESHTOKEN", UsuarioId = usuario.Id, Expiracao = DateTime.UtcNow.AddDays(7) });

            // Act
            var result = await registrarHandler!.Handle(new RegistrarUsuarioCommand("Teste", "teste@teste.com", "Senha@123", "Senha@123"), CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("ACCESSTOKEN", result.Value.AccessToken.Chave);
            Assert.AreEqual("REFRESHTOKEN", result.Value.RefreshToken);
        }

        [TestMethod]
        public async Task AutenticarUsuario_Deve_Retornar_Fail_Quando_Login_Incorreto()
        {
            // Arrange
            var usuario = new Usuario { UserName = "teste@teste.com", Email = "teste@teste.com" };
            userManagerMock!.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(usuario);
            signInManagerMock!.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), true, false))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await autenticarHandler!.Handle(new AutenticarUsuarioCommand("teste@teste.com", "SenhaIncorreta"), CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsFailed);
        }
    }
}
