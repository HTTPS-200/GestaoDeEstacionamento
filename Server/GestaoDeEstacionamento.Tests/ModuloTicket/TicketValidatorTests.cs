using FluentValidation.TestHelper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestaoDeEstacionamento.Testes.Unidade.ModuloTicket
{
    [TestClass]
    [TestCategory("Validação - Ticket")]
    public class TicketValidatorTests
    {
        private CadastrarTicketCommandValidator? cadastrarValidator;
        private EditarTicketCommandValidator? editarValidator;
        private ObterTicketPorNumeroQueryValidator? obterValidator;

        [TestInitialize]
        public void Setup()
        {
            cadastrarValidator = new CadastrarTicketCommandValidator();
            editarValidator = new EditarTicketCommandValidator();
            obterValidator = new ObterTicketPorNumeroQueryValidator();
        }

        [DataTestMethod]
        [DataRow("ABC123", true)]
        [DataRow("", false)]
        [DataRow("TOO_LONG_PLATE", false)]
        [DataRow("ABC@123", false)]
        public void CadastrarTicketCommand_Valido(string placa, bool esperado)
        {
            var command = new CadastrarTicketCommand(placa);
            var result = cadastrarValidator!.TestValidate(command);
            Assert.AreEqual(esperado, result.IsValid);
        }

        [DataTestMethod]
        [DataRow("ABC123", true)]
        [DataRow("", false)]
        [DataRow("TOO_LONG_PLATE", false)]
        [DataRow("ABC@123", false)]
        public void EditarTicketCommand_Valido(string placa, bool esperado)
        {
            var command = new EditarTicketCommand(Guid.NewGuid(), placa, true);
            var result = editarValidator!.TestValidate(command);
            Assert.AreEqual(esperado, result.IsValid);
        }

        [DataTestMethod]
        [DataRow("ABC123", true)]
        [DataRow("", false)]
        [DataRow("TOO_LONG_PLATE", false)]
        [DataRow("ABC@123", false)]
        public void ObterTicketPorNumeroQuery_Valido(string numero, bool esperado)
        {
            var query = new ObterTicketPorNumeroQuery(numero);
            var result = obterValidator!.TestValidate(query);
            Assert.AreEqual(esperado, result.IsValid);
        }
    }
}
