// =================================================================================================
// Academic Research Assistant Portal (ARAP) - API Gateway
// =================================================================================================
// 
// This is the main entry point for the ARAP API Gateway. The gateway serves as the central
// entry point for all client requests and routes them to the appropriate microservices.
// 
// Architecture Overview:
// - API Gateway Pattern: Single entry point for all client requests
// - Microservices: Modular architecture with separate services for different domains
// - Authentication: JWT Bearer tokens with Keycloak integration
// - Documentation: Swagger/OpenAPI for API documentation
// - CORS: Cross-Origin Resource Sharing configured for web client access
// 
// Modules:
// - Research Proposal: Handles research proposal submissions and management
// - Document Review: Manages document review workflows
// - Progress Tracking: Tracks research progress and milestones
// - Academic Integrity: Ensures academic integrity through plagiarism checks
// - Notifications: Handles system notifications and communications
// 
// Author: ARAP Development Team
// Version: 1.0.0
// =================================================================================================

using ARAP.Modules.ResearchProposal.Api;
using ARAP.Modules.DocumentReview.Api;
using ARAP.Modules.ProgressTracking.Api;
using ARAP.Modules.AcademicIntegrity.Api;
using ARAP.Modules.Notifications.Api;
using ARAP.Infrastructure.Outbox;
using Microsoft.IdentityModel.Tokens;

// Create the web application builder with preconfigured defaults
var builder = WebApplication.CreateBuilder(args);

// =================================================================================================
// CORS Configuration
// =================================================================================================
// Configure Cross-Origin Resource Sharing (CORS) to allow web browsers to make requests
// to this API from different domains. This is essential for frontend applications
// hosted on different domains or ports during development and production.
// =================================================================================================
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        // Allow requests from any origin (development setting)
        // In production, consider specifying specific origins for security
        policy.AllowAnyOrigin()
              .AllowAnyMethod()    // Allow all HTTP methods (GET, POST, PUT, DELETE, etc.)
              .AllowAnyHeader();   // Allow all HTTP headers
    });
});

// =================================================================================================
// Core Services Configuration
// =================================================================================================
// Add essential ASP.NET Core services for API functionality
// =================================================================================================

// Add controllers support for API endpoints
builder.Services.AddControllers();

// Add API Explorer for endpoint discovery and metadata
builder.Services.AddEndpointsApiExplorer();

// =================================================================================================
// Swagger/OpenAPI Documentation Configuration
// =================================================================================================
// Configure Swagger for interactive API documentation and testing
// This provides a web-based UI for exploring and testing API endpoints
// =================================================================================================
builder.Services.AddSwaggerGen(c =>
{
    // Define the API information
    c.SwaggerDoc("v1", new() { Title = "Academic Research Assistant Portal API", Version = "v1" });
    
    // =================================================================================================
    // JWT Bearer Authentication Configuration for Swagger
    // =================================================================================================
    // Configure Swagger to accept JWT Bearer tokens for authentication
    // This enables the "Authorize" button in the Swagger UI for testing protected endpoints
    // =================================================================================================
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",                    // Header name for the token
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",                        // Bearer token scheme
        BearerFormat = "JWT",                     // Token format
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your JWT token in the format: Bearer {your token}"
    });
    
    // Apply the security requirement globally to all endpoints
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()  // No scopes required
        }
    });
});

// =================================================================================================
// Transactional Outbox Pattern Configuration
// =================================================================================================
// The outbox pattern ensures reliable event publishing by storing events in the same
// transaction as the business logic, then processing them asynchronously.
// Currently commented out - uncomment when implementing event-driven architecture
// =================================================================================================
// builder.Services.AddOutbox(builder.Configuration);

// =================================================================================================
// Microservice Modules Registration
// =================================================================================================
// Register all microservice modules with their respective dependencies.
// Each module extends the service collection with its own services, configurations,
// and database contexts following the modular architecture pattern.
// =================================================================================================

// Research Proposal Module - Handles research proposal submissions and management
builder.Services.AddResearchProposalModule(builder.Configuration);

// Document Review Module - Manages document review workflows and approvals
builder.Services.AddDocumentReviewModule(builder.Configuration);

// Progress Tracking Module - Tracks research progress and milestone achievements
builder.Services.AddProgressTrackingModule(builder.Configuration);

// Academic Integrity Module - Ensures academic integrity through plagiarism detection
builder.Services.AddAcademicIntegrityModule(builder.Configuration);

// Notifications Module - Handles system notifications and user communications
builder.Services.AddNotificationsModule(builder.Configuration);

// =================================================================================================
// Authentication & Authorization Configuration
// =================================================================================================
// Configure JWT Bearer authentication with Keycloak as the identity provider.
// Authentication can be disabled for development/testing purposes via configuration.
// =================================================================================================

// Check if authentication is disabled via configuration (useful for development)
var disableAuth = builder.Configuration.GetValue<bool>("DisableAuthentication");

if (!disableAuth)
{
    // =================================================================================================
    // Production Authentication Configuration
    // =================================================================================================
    // Configure JWT Bearer authentication with Keycloak integration
    // =================================================================================================
    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            // Keycloak authority URL - the identity provider endpoint
            options.Authority = builder.Configuration["Keycloak:Authority"];
            
            // For development only - set to true in production
            options.RequireHttpsMetadata = false; 
            
            // Configure token validation parameters
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,           // Validate the token issuer
                ValidateAudience = false,        // Keycloak doesn't always include 'aud' claim
                ValidateLifetime = true,         // Ensure token is not expired
                ValidateIssuerSigningKey = true, // Validate the token signature
                NameClaimType = "preferred_username", // Use username as the name claim
                ValidIssuer = builder.Configuration["Keycloak:Authority"] // Expected issuer
            };
        });

    // =================================================================================================
    // Claims Transformation Configuration
    // =================================================================================================
    // Register custom claims transformation to extract Keycloak roles from realm_access claim
    // This ensures proper role-based authorization within the application
    // =================================================================================================
    builder.Services.AddTransient<Microsoft.AspNetCore.Authentication.IClaimsTransformation, ARAP.Api.KeycloakRolesClaimsTransformation>();
    
    // Add authorization services for role-based access control
    builder.Services.AddAuthorization();
}
else
{
    // =================================================================================================
    // Development/Test Authentication Configuration
    // =================================================================================================
    // Configure a test authentication scheme that allows anonymous access
    // This is useful for development and testing without requiring Keycloak
    // =================================================================================================
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "TestScheme";
        options.DefaultChallengeScheme = "TestScheme";
    })
    .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, TestAuthenticationHandler>("TestScheme", options => { });
    
    // Configure authorization to allow all requests (development only)
    builder.Services.AddAuthorization(options =>
    {
        options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
            .RequireAssertion(_ => true)  // Allow all requests
            .Build();
    });
}

// Build the web application with all configured services
var app = builder.Build();

// =================================================================================================
// HTTP Request Pipeline Configuration
// =================================================================================================
// Configure the middleware pipeline that processes incoming HTTP requests.
// The order of middleware is important as each component processes requests
// in the order they are added and responses in reverse order.
// =================================================================================================

// Enable CORS - must be called early to handle preflight requests
app.UseCors(); 

// =================================================================================================
// Swagger/OpenAPI Middleware
// =================================================================================================
// Enable Swagger middleware for API documentation
// This provides interactive API documentation and testing capabilities
// =================================================================================================
app.UseSwagger();

// Configure Swagger UI for interactive API exploration
app.UseSwaggerUI(c =>
{
    // Set the Swagger endpoint URL
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ARAP API v1");
    
    // Set the route prefix for Swagger UI (accessed at /swagger)
    c.RoutePrefix = "swagger";
});

// =================================================================================================
// Security Middleware
// =================================================================================================
// Configure security-related middleware in the correct order
// =================================================================================================

// Redirect HTTP requests to HTTPS (security best practice)
app.UseHttpsRedirection();

// Enable authentication - must come before authorization
app.UseAuthentication();

// Enable authorization - must come after authentication
app.UseAuthorization();

// =================================================================================================
// Routing Configuration
// =================================================================================================
// Map controllers to routes and start the application
// =================================================================================================

// Map controller routes to handle API requests
app.MapControllers();

// Start the web application and begin listening for requests
app.Run();