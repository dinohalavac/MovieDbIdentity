using Duende.IdentityServer.Configuration;
var builder = WebApplication.CreateBuilder(args);

// Register IdentityServer
builder.Services.AddIdentityServer(options =>
    {
        options.UserInteraction.LoginUrl = "/Account/Login";
        options.UserInteraction.ConsentUrl = "/Account/Consent";
        options.UserInteraction.LogoutUrl = "/Account/Logout";
    })
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryIdentityResources(Config.IdentityResources);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies");

builder.Services.Configure<IdentityServerOptions>(options =>
{
    options.UserInteraction.LoginUrl = "/Account/Login";
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Your Angular app URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
// Add Razor Pages for UI
builder.Services.AddRazorPages();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseCors();
app.UseRouting();

app.UseIdentityServer();
app.MapRazorPages();

app.Run();