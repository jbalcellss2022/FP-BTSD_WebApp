using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
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
        //options.Cookie.Expiration = TimeSpan.FromMinutes(30);
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = new PathString("/Login/Login");
        options.LogoutPath = new PathString("/Login/Login");
        options.AccessDeniedPath = new PathString("/Login/Login");
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = new PathString("/Login/Login");
    options.LogoutPath = new PathString("/Login/Login");
    options.AccessDeniedPath = new PathString("/Login/Login");
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

Logger logger = LogManager.GetLogger("");                               // Initialize NLog Logger
LogManager.Configuration.Variables["LoggerFileName"] = "Backend";       // Set NLog filename pre/suffix
LogManager.Configuration.Variables["smptServer"] = "lin135.loading.es"; // Set SMTP Server for NLog
LogManager.Configuration.Variables["smptPort"] = "587";                 // Set SMTP Port for NLog
LogManager.Configuration.Variables["smptEmail"] = "";                   // Set SMTP Email for NLog
LogManager.Configuration.Variables["smptUser"] = "";                    // Set SMTP User for NLog
LogManager.Configuration.Variables["smptPassword"] = "";                // Set SMTP password for NLog

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	builder.WebHost.UseUrls("http://0.0.0.0:7100"); // Set the listening port
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

try
{
    app.UseForwardedHeaders(new ForwardedHeadersOptions  // !Important to get real client IP Address
    {
        ForwardedHeaders = ForwardedHeaders.All,
        RequireHeaderSymmetry = false,
        ForwardLimit = null,
        KnownProxies = { IPAddress.Parse("127.0.0.1"), IPAddress.Parse("185.254.207.147") },
    });

    //app.UseHttpsRedirection();
    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = ctx => {
            const int durationInSeconds = 60 * 60 * 24; 
            ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                "public,max-age=" + durationInSeconds;
        }
    });

    app.UseResponseCompression();
    app.UseCookiePolicy();
    app.UseAuthentication();
    app.UseSession();
    app.UseCors("AllowAll");
    app.UseRouting();
    app.MapRazorPages();
    app.MapDefaultControllerRoute();
    app.UseAuthorization();
    app.Run();

}
catch (Exception ex)
{
    logger.Error(ex, "An error occurred while starting the application.");
}
