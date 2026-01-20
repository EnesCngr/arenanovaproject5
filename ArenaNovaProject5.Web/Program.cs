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

// Register Firestore REST Service with HttpClient
builder.Services.AddHttpClient<FirestoreRestService>();

// Register User Creation Service with HttpClient
builder.Services.AddHttpClient<UserCreationService>();

// Register Firebase Auth Service with HttpClient
builder.Services.AddHttpClient<FirebaseAuthService>();

// Register Auth Services
builder.Services.AddAuthentication();
builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton<AuthSessionService>();
builder.Services.AddScoped<FirebaseAuthStateProvider>();
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider>(sp => sp.GetRequiredService<FirebaseAuthStateProvider>());
builder.Services.AddSingleton<ArenaNovaProject5.Web.Services.KidSessionService>();


// Register Memory Cache
builder.Services.AddMemoryCache();

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
