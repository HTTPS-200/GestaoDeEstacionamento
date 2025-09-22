using GestaoDeEstacionamento.Core.Aplicacao.ModuloAutenticacao.Comands;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloAutenticacao
{
    public sealed class RegistrarUsuarioCommandValidator : AbstractValidator<RegistrarUsuarioCommand>
    {
        public RegistrarUsuarioCommandValidator()
        {
            RuleFor(x => x.NomeCompleto).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Senha).NotEmpty().MinimumLength(6);
            RuleFor(x => x.ConfirmarSenha).Equal(x => x.Senha);
        }
    }

    [TestClass]
    [TestCategory("Testes - Unidade/Validação - Autenticacao")]
    public sealed class AutenticacaoValidatorTests
    {
        private RegistrarUsuarioCommandValidator? validator;

        [TestInitialize]
        public void Setup()
        {
            validator = new RegistrarUsuarioCommandValidator();
        }

        [TestMethod]
        public void Deve_Validar_Comando_Correto()
        {
            var command = new RegistrarUsuarioCommand("Teste", "teste@teste.com", "Senha@123", "Senha@123");
            ValidationResult result = validator!.Validate(command);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Deve_Invalidar_Comando_Com_Email_Invalido()
        {
            var command = new RegistrarUsuarioCommand("Teste", "emailinvalido", "Senha@123", "Senha@123");
            ValidationResult result = validator!.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.PropertyName == "Email"));
        }

        [TestMethod]
        public void Deve_Invalidar_Comando_Senha_Confirmacao_Diferente()
        {
            var command = new RegistrarUsuarioCommand("Teste", "teste@teste.com", "Senha@123", "SenhaDiferente");
            ValidationResult result = validator!.Validate(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.PropertyName == "ConfirmarSenha"));
        }
    }
}
