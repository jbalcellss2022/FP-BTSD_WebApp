using Microsoft.Net.Http.Headers;
using NLog;

var builder = WebApplication.CreateBuilder(args);
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

}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

try
{
    app.UseHttpsRedirection();
    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = ctx => {
            const int durationInSeconds = 60 * 60 * 24; 
            ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                "public,max-age=" + durationInSeconds;
        }
    });
    app.UseRouting();
    app.MapRazorPages();
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "An error occurred while starting the application.");
}
