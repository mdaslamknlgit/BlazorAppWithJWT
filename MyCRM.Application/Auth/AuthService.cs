using MyCRM.Application.Interfaces;

namespace MyCRM.Application.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUnitOfWork unitOfWork,
        ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        var userId = _unitOfWork.Users
            .ValidateCredentials(request.Username, request.Password);

        if (userId is null)
            throw new UnauthorizedAccessException();

        var tokenResult =
            _tokenService.CreateTokens(userId.Value, request.Username);

        _unitOfWork.RefreshTokens.Add(
            tokenResult.RefreshToken,
            userId.Value,
            tokenResult.AccessTokenExpiresAt);

        await _unitOfWork.CommitAsync();

        return new AuthResult
        {
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken,
            AccessTokenExpiresAt = tokenResult.AccessTokenExpiresAt
        };
    }

    public async Task<AuthResult> RefreshAsync(string refreshToken)
    {
        var isValid = _unitOfWork.RefreshTokens.IsValid(refreshToken);

        if (!isValid)
            throw new UnauthorizedAccessException();

        _unitOfWork.RefreshTokens.Revoke(refreshToken);

        var tokenResult =
            _tokenService.CreateTokens(0, string.Empty);

        _unitOfWork.RefreshTokens.Add(
            tokenResult.RefreshToken,
            0,
            tokenResult.AccessTokenExpiresAt);

        await _unitOfWork.CommitAsync();

        return new AuthResult
        {
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken,
            AccessTokenExpiresAt = tokenResult.AccessTokenExpiresAt
        };
    }

    public async Task LogoutAsync(string refreshToken)
    {
        _unitOfWork.RefreshTokens.Revoke(refreshToken);
        await _unitOfWork.CommitAsync();
    }
}
