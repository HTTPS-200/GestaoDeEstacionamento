using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

public class RefreshTokenProvider : IRefreshTokenProvider
{
    private readonly AppDbContext _context;

    public RefreshTokenProvider(AppDbContext context)
    {
        _context = context;
    }

    public RefreshToken GerarRefreshToken(Usuario usuario)
    {
        var randomNumber = new byte[64];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return new RefreshToken
        {
            UsuarioId = usuario.Id,
            Token = Convert.ToBase64String(randomNumber),
            Expiracao = DateTime.UtcNow.AddDays(7),
            Usado = false,
            Revogado = false
        };
    }

    public async Task SalvarRefreshTokenAsync(RefreshToken token)
    {
        await _context.RefreshTokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> ObterRefreshTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.Usuario)
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.Usado && !rt.Revogado);
    }

    public async Task InvalidarRefreshTokenAsync(string token)
    {
        var rt = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        if (rt != null)
        {
            rt.Usado = true;
            rt.Revogado = true;
            await _context.SaveChangesAsync();
        }
    }
}
