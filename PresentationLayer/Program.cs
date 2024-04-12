using Microsoft.Net.Http.Headers;
using NLog;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Logger logger = LogManager.GetLogger("");   // Initialize NLog Logger

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
            const int durationInSeconds = 60 * 60 * 24; // 1 dia
            ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                "public,max-age=" + durationInSeconds;
        }
    });
    app.UseRouting();
    app.UseAuthorization();
    app.MapRazorPages();
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "An error occurred while starting the application.");
}
