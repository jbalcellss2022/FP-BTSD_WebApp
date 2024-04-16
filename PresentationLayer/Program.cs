using BusinessLogicLayer;
using DeviceDetectorNET.Parser.Device;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using NLog;
using System.IO.Compression;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });
builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; options.Providers.Add<GzipCompressionProvider>(); });
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Cookie Management
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        options.LoginPath = new PathString("/SignIn/Login");
        options.LogoutPath = new PathString("/SignIn/Login");
        options.AccessDeniedPath = new PathString("/SignIn/Login");
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.LoginPath = new PathString("/SignIn/Login");
    options.LogoutPath = new PathString("/SignIn/Login");
    options.AccessDeniedPath = new PathString("/SignIn/Login");
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.Configure<ForwardedHeadersOptions>(options => { options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; });

/*
builder.Services.AddDbContext<BBDDContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionString"))
    .EnableSensitiveDataLogging(true)
    .EnableDetailedErrors());
*/

BLServiceCollection.GetServiceCollection("connString", builder.Services, (IConfigurationRoot)builder.Configuration);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
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
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => { opts.ResourcesPath = "Resources"; });

var app = builder.Build();

Logger logger = LogManager.GetLogger("");                               // Get NLog logger
LogManager.Configuration.Variables["LoggerFileName"] = "Backend";       // Set NLog filename pre/suffix
LogManager.Configuration.Variables["smptServer"] = "lin135.loading.es"; // Set SMTP Server for NLog
LogManager.Configuration.Variables["smptPort"] = "587";                 // Set SMTP Port for NLog
LogManager.Configuration.Variables["smptEmail"] = "";                   // Set SMTP Email for NLog
LogManager.Configuration.Variables["smptUser"] = "";                    // Set SMTP User for NLog
LogManager.Configuration.Variables["smptPassword"] = "";                // Set SMTP password for NLog

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	//app.UseExceptionHandler("/Error");  // Uses a friendly error page in production mode
	app.UseDeveloperExceptionPage();  // Uses detailed exception page in development mode
}
else
{
	builder.WebHost.UseUrls("http://0.0.0.0:7100"); // Set the listening port for WebHost production mode
	app.UseExceptionHandler("/Error");  // Uses a friendly error page in production mode
	app.UseHsts(); // Use HSTS in production mode
}

try
{
    /*
    // Middleware to show specific error pages
	app.UseStatusCodePages(async context =>
	{
		var request = context.HttpContext.Request;
		var response = context.HttpContext.Response;
		if (response.StatusCode == 404)
		{
			response.Redirect("/Home/Error");   // Redirect to error page if status code is 404
		}
	});
    */

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
    
    app.UseResponseCompression();       // Middleware to enable response compression
	app.UseCookiePolicy();              // Middleware to manage cookies
    app.UseAuthentication();            // Middleware to manage authentication
    app.UseSession();                   // Middleware to manage session
    app.UseCors("AllowAll");            // Middleware to manage CORS
    app.UseRouting();                   // Middleware to manage routing
    app.MapRazorPages();                // Middleware to manage Razor Pages
    app.MapDefaultControllerRoute();    // Middleware to manage default controller route
    app.UseAuthorization();             // Middleware to manage authorization

    app.Run();
}
catch (Exception ex)
{
	logger.Error(ex, message: "An error occurred while starting the application.");
}
