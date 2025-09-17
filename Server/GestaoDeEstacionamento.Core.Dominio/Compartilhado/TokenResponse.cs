namespace GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao
{
    public record TokenResponse(AccessToken AccessToken, string RefreshToken);
}
