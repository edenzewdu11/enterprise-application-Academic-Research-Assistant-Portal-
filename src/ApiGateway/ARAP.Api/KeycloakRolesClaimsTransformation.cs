using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace ARAP.Api;

/// <summary>
/// Transforms Keycloak authentication claims by extracting user roles from the realm_access claim
/// and adding them as standard role claims to the claims principal.
/// </summary>
/// <remarks>
/// This class is essential for proper role-based authorization in the ARAP system.
/// Keycloak returns roles in a nested JSON structure within the realm_access claim,
/// which needs to be extracted and added as individual Role claims for ASP.NET Core
/// authorization policies to work correctly.
/// </remarks>
public class KeycloakRolesClaimsTransformation : IClaimsTransformation
{
    private readonly ILogger<KeycloakRolesClaimsTransformation> _logger;

    /// <summary>
    /// Initializes a new instance of the KeycloakRolesClaimsTransformation class.
    /// </summary>
    /// <param name="logger">Logger instance for recording transformation events and errors</param>
    public KeycloakRolesClaimsTransformation(ILogger<KeycloakRolesClaimsTransformation> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Transforms the claims principal by extracting Keycloak roles from the realm_access claim
    /// and adding them as individual Role claims.
    /// </summary>
    /// <param name="principal">The incoming claims principal from Keycloak authentication</param>
    /// <returns>A task that represents the asynchronous transformation operation, 
    /// returning the enhanced claims principal with role claims</returns>
    /// <remarks>
    /// The method performs the following steps:
    /// 1. Validates that the claims identity exists and is authenticated
    /// 2. Logs all existing claims for debugging purposes
    /// 3. Extracts the realm_access claim which contains roles in JSON format
    /// 4. Parses the JSON and adds each role as a separate Role claim
    /// 5. Logs the final set of role claims for verification
    /// </remarks>
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        // Extract the claims identity from the principal
        var claimsIdentity = principal.Identity as ClaimsIdentity;
        
        // Validate that we have a valid, authenticated identity
        if (claimsIdentity == null || !claimsIdentity.IsAuthenticated)
        {
            _logger.LogWarning("Claims identity is null or not authenticated");
            return Task.FromResult(principal);
        }

        // Log the start of transformation for debugging purposes
        _logger.LogInformation("=== Claims Transformation Started ===");
        _logger.LogInformation($"All claims: {string.Join(", ", principal.Claims.Select(c => $"{c.Type}={c.Value}"))}");

        // Check for existing role claims (don't skip - we need to ensure Keycloak roles are added)
        var existingRoles = principal.FindAll(ClaimTypes.Role).ToList();
        _logger.LogInformation($"Existing role claims: {string.Join(", ", existingRoles.Select(c => c.Value))}");

        // Extract roles from the realm_access claim (Keycloak-specific format)
        // The realm_access claim contains a JSON object with a "roles" array
        var realmAccessClaim = principal.FindFirst("realm_access");
        if (realmAccessClaim != null)
        {
            _logger.LogInformation($"Found realm_access claim: {realmAccessClaim.Value}");
            try
            {
                // Parse the JSON content of the realm_access claim
                using var realmAccess = JsonDocument.Parse(realmAccessClaim.Value);
                
                // Look for the "roles" property in the JSON
                if (realmAccess.RootElement.TryGetProperty("roles", out var rolesElement))
                {
                    // Iterate through each role in the roles array
                    foreach (var role in rolesElement.EnumerateArray())
                    {
                        var roleValue = role.GetString();
                        
                        // Add each non-empty role as a new Role claim
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
                // Log any parsing errors but don't fail the authentication
                _logger.LogError(ex, "Error parsing realm_access claim");
            }
        }
        else
        {
            _logger.LogWarning("No realm_access claim found");
        }

        // Log the final state of all role claims for verification
        var finalRoles = principal.FindAll(ClaimTypes.Role).ToList();
        _logger.LogInformation($"Final role claims: {string.Join(", ", finalRoles.Select(c => c.Value))}");
        _logger.LogInformation("=== Claims Transformation Complete ===");

        // Return the enhanced principal with added role claims
        return Task.FromResult(principal);
    }
}
