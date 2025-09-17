using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
public class RefreshTokenProvider
{
    private readonly IRefreshTokenProvider repositorio;

    public RefreshTokenProvider(IRefreshTokenProvider repositorio)
    {
        this.repositorio = repositorio;
    }

    public async Task<RefreshToken> CriarERegistrarRefreshTokenAsync(Usuario usuario)
    {
        var token = repositorio.GerarRefreshToken(usuario);
        await repositorio.SalvarRefreshTokenAsync(token);
        return token;
    }

    public Task<RefreshToken?> ValidarRefreshTokenAsync(string token)
    {
        return repositorio.ObterRefreshTokenAsync(token);
    }

    public Task InvalidarRefreshTokenAsync(string token)
    {
        return repositorio.InvalidarRefreshTokenAsync(token);
    }
}
