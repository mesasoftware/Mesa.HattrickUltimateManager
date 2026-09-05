namespace Mesa.HUM.Presentation.Facades.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Mesa.HUM.Domain.Common;
    using Mesa.HUM.Presentation.Facades.Contracts.Authorization;

    public interface IAuthorizationFacade
    {
        Task<Result<GetAccessTokenResultDto>> GetAccessTokenAsync ( RequestTokenDto requestToken , string verifier , CancellationToken cancellationToken = default );

        Task<Result<GetAuthorizationUrlResultDto>> GetAuthorizationUrlAsync ( string [ ]? scopes , CancellationToken cancellationToken = default );
    }
}