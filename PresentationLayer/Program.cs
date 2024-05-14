using BusinessLogicLayer.Classes;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Entities.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using NLog;
using NLog.Web;
using SecurityHubs.Hubs;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Error);
builder.Host.UseNLog();

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

services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
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

services.AddDbContext<BBDDContext>(options => options.UseSqlServer(builder.Configuration["Database:ConnectionString"])
    .EnableSensitiveDataLogging(true)
    .EnableDetailedErrors());

BLServiceCollection.GetServiceCollection(builder.Services);

services.AddControllersWithViews();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
}

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

services.AddSignalR(config => {
    config.EnableDetailedErrors = true;
    config.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
    config.KeepAliveInterval = TimeSpan.FromSeconds(30);
});

services.AddSession();
services.AddHttpContextAccessor();
services.AddMvc();

var app = builder.Build();

Logger Logger = LogManager.GetLogger("");                               // Get NLog logger
LogManager.Configuration.Variables["LoggerFileName"] = "QRFYBackend";   // Set NLog filename pre/suffix

SqlConnectionStringBuilder SQLbuilder = new SqlConnectionStringBuilder(builder.Configuration["Database:ConnectionString"]);
string DB_Server = SQLbuilder.DataSource;
string DB_Catalog = SQLbuilder.InitialCatalog;
string DB_UserId = SQLbuilder.UserID;
string DB_Pass = SQLbuilder.Password;

LogManager.Configuration.Variables["DB_Server"] = DB_Server;    // Set DB Server for NLog
LogManager.Configuration.Variables["DB_Catalog"] = DB_Catalog;  // Set Database for NLog
LogManager.Configuration.Variables["DB_UserId"] = DB_UserId;    // Set DB UserId NLog
LogManager.Configuration.Variables["DB_Pass"] = DB_Pass;        // Set DB Password for NLog

LogManager.Configuration.Variables["TargetMail"] = builder.Configuration["NLog:TargetMail"];  // Set Target Email for NLog

LogManager.Configuration.Variables["smtpServer"] = builder.Configuration["EmailSettings:smtpServer"];  // Set SMTP Server for NLog
LogManager.Configuration.Variables["smtpPort"] = builder.Configuration["EmailSettings:smtpPort"];      // Set SMTP Port for NLog
LogManager.Configuration.Variables["smtpEmail"] = builder.Configuration["EmailSettings:smtpEmail"];    // Set SMTP Email for NLog
LogManager.Configuration.Variables["smtpUser"] = builder.Configuration["EmailSettings:smtpUser"];      // Set SMTP User for NLog
LogManager.Configuration.Variables["smtpPass"] = builder.Configuration["EmailSettings:smtpPass"];  // Set SMTP password for NLog

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
    // Register the exception handling middleware
    //app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Device Detector Start
    using (var serviceScope = app.Services.CreateScope())
    {
        var servicesDD = serviceScope.ServiceProvider;
        var DeviceDetectorService = servicesDD.GetRequiredService<IUserDDService>();
        DeviceDetectorService!.StartDeviceDetector();
    }

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

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHub<SecurityHub>("/securityhub", options =>
        {
            options.Transports = HttpTransportType.LongPolling | HttpTransportType.ServerSentEvents;
            options.LongPolling.PollTimeout = TimeSpan.FromSeconds(180);
            options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(180);
        });
    });

    app.MapControllerRoute( name: "default", pattern: "{controller=Dashboard}/{action=Index}/{id?}" ); // Middleware to manage default controller route 
	app.Run();
}
catch (Exception ex)
{
    if (!Debugger.IsAttached)
    {
        Logger.Error(System.Reflection.MethodBase.GetCurrentMethod()!.Name + "[M]: " + ex.Message ?? "" + "[StackT]: " + ex.StackTrace ?? "" + "[HLink]: " + ex.HelpLink ?? "" + "[HResult]: " + ex.HResult ?? "" + "[Source]: " + ex.Source ?? "" + ex.Data ?? "" + "[InnerE]: " + ex.InnerException!.Message ?? "");
    }
}
