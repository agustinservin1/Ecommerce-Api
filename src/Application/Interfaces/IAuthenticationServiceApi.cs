using Application.Models.Request;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAuthenticationServiceApi
    {
        User? ValidateUser(CredentialRequest credentialRequest);
        string AuthenticateCredentials(CredentialRequest credentialRequest);
    }
}
