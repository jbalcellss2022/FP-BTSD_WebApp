using BusinessLogicLayer;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using NLog;
using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Resources;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });
services.AddResponseCompression(
    options => {
		options.EnableForHttps = true;
		options.Providers.Add<BrotliCompressionProvider>();
		options.Providers.Add<GzipCompressionProvider>();
	});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
	options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
	options.Level = CompressionLevel.SmallestSize;
});

builder.Services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

services.AddHttpClient();                                                   // HttpClient
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();         // HttpContextAccessor

services.AddScoped<IUserDDService, UserDDService>();         

builder.Services.AddLocalization();

// Cookie Management
services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
});

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.LoginPath = new PathString("/SignIn/Login");
        options.LogoutPath = new PathString("/SignIn/Login");
        options.AccessDeniedPath = new PathString("/SignIn/Login");
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = new PathString("/SignIn/Login");
    options.LogoutPath = new PathString("/SignIn/Login");
    options.AccessDeniedPath = new PathString("/SignIn/Login");
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

services.Configure<ForwardedHeadersOptions>(options => { options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; });

services.AddDbContext<BBDDContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
    .EnableSensitiveDataLogging(true)
    .EnableDetailedErrors());

BLServiceCollection.GetServiceCollection(builder.Services, (IConfigurationRoot)builder.Configuration);

services.AddRazorPages();
services.AddControllersWithViews();
services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
    builder =>
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed((host) => true);
    });
});
services.AddSession();
services.AddHttpContextAccessor();
services.AddMvc();

var app = builder.Build();

Logger logger = LogManager.GetLogger("");                               // Get NLog logger
LogManager.Configuration.Variables["LoggerFileName"] = "QRFYBackend";       // Set NLog filename pre/suffix
LogManager.Configuration.Variables["smptServer"] = "lin135.loading.es"; // Set SMTP Server for NLog
LogManager.Configuration.Variables["smptPort"] = "587";                 // Set SMTP Port for NLog
LogManager.Configuration.Variables["smptEmail"] = "";                   // Set SMTP Email for NLog
LogManager.Configuration.Variables["smptUser"] = "";                    // Set SMTP User for NLog
LogManager.Configuration.Variables["smptPassword"] = "";                // Set SMTP password for NLog

logger.Info("init");
logger.Warn("warn");
logger.Error("Error");

// Device Detector Start
using (var serviceScope = app.Services.CreateScope())
{
    var servicesDD = serviceScope.ServiceProvider;
    var DeviceDetectorService = servicesDD.GetRequiredService<IUserDDService>();

    DeviceDetectorService!.StartDeviceDetector();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	//app.UseExceptionHandler("/Error");  // Uses a friendly error page in production mode
	app.UseDeveloperExceptionPage();  // Uses detailed exception page in development mode
}
else
{
	app.UseResponseCompression();                       // Middleware to enable response compression
	builder.WebHost.UseUrls("http://0.0.0.0:7100");     // Set the listening port for WebHost production mode
	app.UseExceptionHandler("/Error");                  // Uses a friendly error page in production mode
	app.UseHsts();                                      // Use HSTS in production mode
}

try
{
    // Middleware to forward headers
	app.UseForwardedHeaders(new ForwardedHeadersOptions  // !Important to get real client IP Address
    {
        ForwardedHeaders = ForwardedHeaders.All,
        RequireHeaderSymmetry = false,
        ForwardLimit = null,
        KnownProxies = { IPAddress.Parse("127.0.0.1"), IPAddress.Parse("79.143.89.216") },
    });

    // Middleware to serve static files and enable response compression
    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = ctx => {
            const int durationInSeconds = 60 * 60 * 24; 
            ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                "public,max-age=" + durationInSeconds;
        }
    });

	var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("es") }; // Middleware to manage request localization
	app.UseRequestLocalization(new RequestLocalizationOptions
	{
		DefaultRequestCulture = new RequestCulture("en"),
		SupportedCultures = supportedCultures,
		SupportedUICultures = supportedCultures,
	});

	app.UseCookiePolicy();              // Middleware to manage cookies
    app.UseAuthentication();            // Middleware to manage authentication
    app.UseSession();                   // Middleware to manage session
    app.UseCors("AllowAll");            // Middleware to manage CORS
    app.UseRouting();                   // Middleware to manage routing
    app.UseAuthorization();             // Middleware to manage authorization
    app.MapRazorPages();                // Middleware to manage Razor Pages

    app.MapControllerRoute( name: "default", pattern: "{controller=Dashboard}/{action=Index}/{id?}" ); // Middleware to manage default controller route 
	app.Run();
}
catch (Exception ex)
{
	logger.Error(ex, message: "An error occurred while starting the application.");
}
