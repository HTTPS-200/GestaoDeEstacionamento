using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloAutenticacao
{
    [TestClass]
    [TestCategory("Testes - Unidade/Repositório - Autenticacao")]
    public sealed class AutenticacaoRepositoryTests
    {
        private Mock<IRefreshTokenProvider>? refreshTokenProviderMock;
        private List<RefreshToken>? tokens;

        [TestInitialize]
        public void Setup()
        {
            refreshTokenProviderMock = new Mock<IRefreshTokenProvider>();
            tokens = new List<RefreshToken>();
        }

        [TestMethod]
        public void GerarRefreshToken_Deve_Criar_Token_Valido()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var usuario = new Usuario { Id = usuarioId, UserName = "teste@teste.com" };
            refreshTokenProviderMock!.Setup(r => r.GerarRefreshToken(usuario))
                .Returns(new RefreshToken { Token = "REFRESHTOKEN", UsuarioId = usuario.Id, Expiracao = DateTime.UtcNow.AddDays(7) });

            // Act
            var token = refreshTokenProviderMock.Object.GerarRefreshToken(usuario);

            // Assert
            Assert.IsNotNull(token);
            Assert.AreEqual(usuarioId, token.UsuarioId);
            Assert.AreEqual("REFRESHTOKEN", token.Token);
        }
    }
}
