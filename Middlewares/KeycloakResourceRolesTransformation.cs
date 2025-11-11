using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;


public class KeycloakResourceRolesTransformation : IClaimsTransformation
{
    private const string ResourceAccessClaim = "resource_access";
    private const string RolesKey = "roles";
    private readonly string _clientId;

    // Injeta IConfiguration para obter o Client ID da API
    public KeycloakResourceRolesTransformation(IConfiguration configuration)
    {
        _clientId = configuration.GetSection("Keycloak")["Audience"]
                    ?? throw new ArgumentNullException("Keycloak:Audience não configurado.");
    }

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        // Tenta encontrar o claim principal 'resource_access'
        var resourceAccessClaim = principal.FindFirst(ResourceAccessClaim);

        if (resourceAccessClaim == null)
        {
            return Task.FromResult(principal); // Sem resource_access, sem roles de cliente para adicionar
        }

        // 1. Deserializa o valor do claim 'resource_access' (que é um JSON)
        using var document = JsonDocument.Parse(resourceAccessClaim.Value);

        // 2. Tenta obter o objeto do seu cliente
        if (document.RootElement.TryGetProperty(_clientId, out var clientElement))
        {
            // 3. Tenta obter o array de 'roles' dentro do seu cliente
            if (clientElement.TryGetProperty(RolesKey, out var rolesElement) && rolesElement.ValueKind == JsonValueKind.Array)
            {
                var identity = (ClaimsIdentity)principal.Identity;

                // 4. Adiciona cada role como um ClaimTypes.Role, que o // [Authorize(Roles="post")] espera
                foreach (var role in rolesElement.EnumerateArray())
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.GetString()!));
                }
            }
        }

        return Task.FromResult(principal);
    }
}