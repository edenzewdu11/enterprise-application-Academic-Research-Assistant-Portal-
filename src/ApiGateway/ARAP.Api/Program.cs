using ARAP.Modules.ResearchProposal.Api;
using ARAP.Modules.DocumentReview.Api;
using ARAP.Modules.ProgressTracking.Api;
using ARAP.Modules.AcademicIntegrity.Api;
using ARAP.Modules.Notifications.Api;
using ARAP.Infrastructure.Outbox;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy to accept requests from any browser
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Academic Research Assistant Portal API", Version = "v1" });
    
    // Add JWT Bearer Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your JWT token in the format: Bearer {your token}"
    });
    
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
            Array.Empty<string>()
        }
    });
});

// Register Transactional Outbox Pattern
builder.Services.AddOutbox(builder.Configuration);

// Register Modules
builder.Services.AddResearchProposalModule(builder.Configuration);
builder.Services.AddDocumentReviewModule(builder.Configuration);
builder.Services.AddProgressTrackingModule(builder.Configuration);
builder.Services.AddAcademicIntegrityModule(builder.Configuration);
builder.Services.AddNotificationsModule(builder.Configuration);

// Add Authentication & Authorization (JWT Bearer with Keycloak)
// To disable authentication for testing, set "DisableAuthentication": true in appsettings.json
var disableAuth = builder.Configuration.GetValue<bool>("DisableAuthentication");

if (!disableAuth)
{
    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = builder.Configuration["Keycloak:Authority"];
            options.RequireHttpsMetadata = false; // For development only
            
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false, // Keycloak doesn't always include 'aud' claim
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                NameClaimType = "preferred_username",
                ValidIssuer = builder.Configuration["Keycloak:Authority"]
            };
        });

    // Add claims transformation to extract Keycloak roles
    builder.Services.AddTransient<Microsoft.AspNetCore.Authentication.IClaimsTransformation, ARAP.Api.KeycloakRolesClaimsTransformation>();
    
    builder.Services.AddAuthorization();
}
else
{
    // Allow anonymous access for all endpoints (development only)
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "TestScheme";
        options.DefaultChallengeScheme = "TestScheme";
    })
    .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, TestAuthenticationHandler>("TestScheme", options => { });
    
    builder.Services.AddAuthorization(options =>
    {
        options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
            .RequireAssertion(_ => true)
            .Build();
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(); // Enable CORS

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ARAP API v1");
    c.RoutePrefix = "swagger"; // Swagger UI at /swagger
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();