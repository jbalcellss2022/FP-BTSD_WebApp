using Asp.Versioning;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using Unchase.Swashbuckle.AspNetCore.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Accessing IConfiguration and IWebHostEnvironment from the builder
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddMvc(options => { options.RespectBrowserAcceptHeader = true; }); // false by default
builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddControllers();

// Configure API IP Rate Limit middelware
builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("APIKeySettings:Secret")!);

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true, // When receiving a token, check that it is still valid.

            // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time
            // when validating the lifetime. As we're creating the tokens locally and validating them on the same
            // machines which should have synchronized time, this can be set to zero. Where external tokens are
            // used, some leeway here could be useful.

            ClockSkew = TimeSpan.Zero
        };
    });

// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (apiDesc.HttpMethod == null) return false;
        return true;
    });

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "QRFY RESTful API",
        Version = "v1.0",
        Description =
          "<strong>We are glad to have your here! In our developer\'s hub you\'ll find everything you need to interact with our platform.</strong> <br /><br />" +
          "<p class=\"imgp\"><img class=\"JWTBearerImg\" src=\"/images/ODM-Main.jpg\" alt =\"ODM main image\"></p><br /> <br />" +
          "The QRFY Web API is organized around REST, using HTTP responses code to keep you informed about what\'s going on. Our endpoints will return metada in Json format. All the \"List\" methods are GET requests and can be paginated thus you can get more pages if needed by query params i.e. <strong>?page=2</strong>.  <br /> <br />All requests are validated against an API JWT Bearer Token. You can obtain it manually from the \"authenticate\" Web API method (as you\'ll see in documented below).  <br /> <br />We tried to keep the documentation as clear and simple as possible. Thus you can test our endpoints with your own API Token and see the responses code directly. Additionally, if you wish, you can use the <strong><a href=\"https://www.postman.com\" target=\"_blank\">POSTMAN</a></strong> program to test all available methods." +
          "",

        TermsOfService = new Uri("https://api.qrfy.es/license_&_terms.html"),
        License = new OpenApiLicense
        {
            Name = "Use under MIT License",
            Url = new Uri("https://api.qrfy.es/license_&_terms.html"),
        },
        Extensions = new Dictionary<string, IOpenApiExtension>
                    {
                        // Logo Extension
                        {"x-logo", new OpenApiObject
                            {
                                {"url", new OpenApiString("https://api.qrfy.es/images/Logo_ODM_2.png")},
                                { "altText", new OpenApiString("API Logo")}
                            }
                        },
                    }
    });

    c.OperationFilter<SwaggerFilterOperationAuthorizationHeader>();

    c.AddSecurityDefinition("JWT Bearer Token", new OpenApiSecurityScheme
    {
        Description = "Use a JWT Bearer Token to communicate with the QRFY RESTful API. Notice! Always enter the token in the following format <strong>{Bearer eyJhbGciOiJIUzI1NiIs...}</strong><br /><br />" +
        "JSON Web Token (<strong><a href=\"https://jwt.io/introduction/\" target =\"_blank\">JWT</a></strong>) is an open standard <strong><a href=\"https://tools.ietf.org/html/rfc7519\" target=\"_blank\">RFC 7519</a></strong> that defines a compact and self-contained way for securely transmitting information between parties as a JSON object. This information is verified and trusted digitally signed using a secret with the HMAC algorithm.<br /> <br />" +
        "<p class=\"imgp\"><img class=\"JWTBearerImg\" src=\"https://cdn2.auth0.com/docs/media/articles/api-auth/client-credentials-grant.png\" alt=\"JWT Bearer Token diagram\" width=\"600\" height =\"228\"></p>" +
        "The output is three Base64-URL strings separated by dots that can be easily passed in HTML and HTTP environments, while being more compact when compared to XML-based standards such as SAML. The following shows a JWT that has the previous header and payload encoded, and it is signed with a secret." +
        "<p class=\"imgp\"><img class=\"JWTBearerImg\" src=\"https://cdn.auth0.com/content/jwt/encoded-jwt3.png\" alt=\"JWT Bearer Token coded sample\" width=\"600\" height =\"138\"></p><br /> <br />"
        ,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "JWT Bearer Token" } }, Array.Empty<string>()
        }
        });

    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // ############ FILTERS ##############################################################################################################################

    // Add filters to fix enums
    c.AddEnumsWithValuesFixFilters();

    // or configured:
    c.AddEnumsWithValuesFixFilters(o =>
    {
        o.ApplySchemaFilter = true;             // add schema filter to fix enums (add 'x-enumNames' for NSwag) in schema
        o.ApplyParameterFilter = true;          // add parameter filter to fix enums (add 'x-enumNames' for NSwag) in schema parameters
        o.ApplyDocumentFilter = true;           // add document filter to fix enums displaying in swagger document
        o.IncludeDescriptions = true;           // add descriptions from DescriptionAttribute or xml-comments to fix enums (add 'x-enumDescriptions' for schema extensions) for applied filters
        o.DescriptionSource = DescriptionSources.DescriptionAttributesThenXmlComments; // get descriptions from DescriptionAttribute then from xml-comments
        o.IncludeXmlCommentsFrom(xmlPath);      // get descriptions from xml-file comments on the specified path should use "options.IncludeXmlComments(xmlFilePath);" before
    });
});

builder.Services.AddSwaggerGenNewtonsoftSupport();      // explicit opt-in - needs to be placed after AddSwaggerGen()


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = ["index.html"] });
app.UseStaticFiles();
app.UseIpRateLimiting();

// Swaggerbuckle. Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger(c => { c.RouteTemplate = "/apidocs/{documentName}/qrfyapi.json"; });  // For Local IIS
app.MapSwagger().RequireAuthorization();

// Enable OPENAPI middleware UI.
app.UseSwaggerUI(c => {
    c.DocumentTitle = "QRFY RESTful API v1";                                         // Html Title
    c.DefaultModelRendering(ModelRendering.Model);
    c.DisplayRequestDuration();
    c.DocExpansion(DocExpansion.None);
    c.EnableDeepLinking();
    c.EnableFilter();
    c.ShowExtensions();
    c.EnableValidator();
    c.InjectStylesheet("custom-ui/custom.css");                                 // Inject custom CSS style
    c.SwaggerEndpoint("/apidocs/v1/qrfyapi.json", "QRFY RESTful API v1");       // For Local IIS
    c.RoutePrefix = "apidocs";                                                  // For Local IIS or ... string.Empty;
});

// Enable REDOC middleware UI.
app.UseReDoc(c => {
    c.DocumentTitle = "QRFY RESTful API v1";                                         // Html Title
    c.RoutePrefix = "apidocs-redoc";
    c.SpecUrl("/apidocs/v1/qrfyapi.json");
    c.InjectStylesheet("/apidocs-redoc/custom-ui/custom.css");
    c.EnableUntrustedSpec();
    c.ScrollYOffset(10);
    c.HideDownloadButton();
    c.ExpandResponses("200");
});

app.Run();
