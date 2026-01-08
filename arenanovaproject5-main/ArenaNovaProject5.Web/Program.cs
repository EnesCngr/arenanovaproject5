using ArenaNovaProject5.Web.Components;
using ArenaNovaProject5.Web.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register MudBlazor services
builder.Services.AddMudServices();

// Register Firebase Service
builder.Services.AddScoped<FirebaseService>();

// Register Firebase Auth Service with HttpClient
builder.Services.AddHttpClient<FirebaseAuthService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthSessionService>();
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider, FirebaseAuthStateProvider>();


// Auth (Blazor Authorization)
builder.Services.AddAuthorizationCore();

// Session state
builder.Services.AddScoped<AuthSessionService>();

// Blazor auth state provider
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider, FirebaseAuthStateProvider>();


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
