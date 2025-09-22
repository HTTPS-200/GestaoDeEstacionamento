using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloAutenticacao
{
    [TestClass]
    [TestCategory("Testes - Unidade - Domínio - Autenticacao")]
    public sealed class AutenticacaoTests
    {
        [TestMethod]
        public void RefreshToken_Deve_Ser_Revogado()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var token = new RefreshToken { Token = "TOKEN1", UsuarioId = usuarioId, Usado = false, Revogado = false };

            // Act
            token.Usado = true;
            token.Revogado = true;

            // Assert
            Assert.IsTrue(token.Usado);
            Assert.IsTrue(token.Revogado);
        }

        [TestMethod]
        public void Usuario_Deve_Ter_Id_Generado()
        {
            // Arrange & Act
            var usuario = new Usuario { UserName = "teste@teste.com", Email = "teste@teste.com" };

            // Assert
            Assert.AreNotEqual(Guid.Empty, usuario.Id);
        }
    }
}
