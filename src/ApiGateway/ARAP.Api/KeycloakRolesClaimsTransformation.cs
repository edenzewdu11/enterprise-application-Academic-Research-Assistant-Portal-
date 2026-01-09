using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace ARAP.Api;

public class KeycloakRolesClaimsTransformation : IClaimsTransformation
{
    private readonly ILogger<KeycloakRolesClaimsTransformation> _logger;

    public KeycloakRolesClaimsTransformation(ILogger<KeycloakRolesClaimsTransformation> logger)
    {
        _logger = logger;
    }

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var claimsIdentity = principal.Identity as ClaimsIdentity;
        if (claimsIdentity == null || !claimsIdentity.IsAuthenticated)
        {
            _logger.LogWarning("Claims identity is null or not authenticated");
            return Task.FromResult(principal);
        }

        _logger.LogInformation("=== Claims Transformation Started ===");
        _logger.LogInformation($"All claims: {string.Join(", ", principal.Claims.Select(c => $"{c.Type}={c.Value}"))}");

        // Don't skip if roles already exist - we need to ensure Keycloak roles are added
        var existingRoles = principal.FindAll(ClaimTypes.Role).ToList();
        _logger.LogInformation($"Existing role claims: {string.Join(", ", existingRoles.Select(c => c.Value))}");

        // Extract roles from realm_access claim
        var realmAccessClaim = principal.FindFirst("realm_access");
        if (realmAccessClaim != null)
        {
            _logger.LogInformation($"Found realm_access claim: {realmAccessClaim.Value}");
            try
            {
                using var realmAccess = JsonDocument.Parse(realmAccessClaim.Value);
                if (realmAccess.RootElement.TryGetProperty("roles", out var rolesElement))
                {
                    foreach (var role in rolesElement.EnumerateArray())
                    {
                        var roleValue = role.GetString();
                        if (!string.IsNullOrEmpty(roleValue))
                        {
                            _logger.LogInformation($"Adding role claim: {roleValue}");
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, roleValue));
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("No 'roles' property found in realm_access");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing realm_access claim");
            }
        }
        else
        {
            _logger.LogWarning("No realm_access claim found");
        }

        var finalRoles = principal.FindAll(ClaimTypes.Role).ToList();
        _logger.LogInformation($"Final role claims: {string.Join(", ", finalRoles.Select(c => c.Value))}");
        _logger.LogInformation("=== Claims Transformation Complete ===");

        return Task.FromResult(principal);
    }
}
