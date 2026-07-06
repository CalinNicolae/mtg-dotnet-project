using Blazored.LocalStorage;
using Microsoft.EntityFrameworkCore;
using MTGProject.Components;
using MTGProject.Models;
using MTGProject.Services;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register HttpClient for dependency injection
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure DbContext using a factory instead of the regular AddDbContext.
builder.Services.AddDbContextFactory<MyDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Register services
builder.Services.AddScoped<CardService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<FilterLocalStorageService>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();