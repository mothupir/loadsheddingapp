using loadsheddingapp.Services;
using loadsheddingapp.Models;
using loadsheddingapp.Repository;
using Microsoft.EntityFrameworkCore;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IJokeRepository, JokeRepository>();


builder.Services.AddDbContext<DataContext>();



SecretsManagerService service = new SecretsManagerService();

var cert_secret = service.getSecret(builder.Configuration["CertSecretID"]);


builder.WebHost.UseKestrel().ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.Listen(IPAddress.Any, 443, listenOptions =>
    {
        listenOptions.UseHttps("/home/ubuntu/domain.com.pfx", cert_secret);
    });
});





builder.Services.AddSingleton<ISecretsManagerService, SecretsManagerService>();


builder.Logging.AddAWSProvider();

builder.Services
    .AddAuth0WebAppAuthentication(options => {
        options.Domain = builder.Configuration["Auth0:Domain"];
        options.ClientId = builder.Configuration["Auth0:ClientId"];
    });





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.MapControllerRoute(
    name: "Login",
    pattern: "{controller=Account}/{action=Index}/{id?}");


app.Run();
