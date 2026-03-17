using Microservices.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddAuthenticationExt(builder.Configuration);
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Yarp (Gateway)");
app.MapReverseProxy();

app.Run();
