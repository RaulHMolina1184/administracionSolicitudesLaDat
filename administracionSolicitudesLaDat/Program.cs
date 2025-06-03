using BusinessLogic;
using DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
});

// Configuraciones desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var encryptionKey = builder.Configuration["Security:EncryptionKey"];

// Inyectar string de conexión en capa datos, inyectar UsuarioService (usa UsuarioDA internamente)
builder.Services.AddSingleton(new UsuarioDA(connectionString));
builder.Services.AddSingleton<EncryptionService>(sp => new EncryptionService(encryptionKey));

// Registrar `UsuarioService`, asegurándose de que reciba `UsuarioDA` y `EncryptionService`
builder.Services.AddSingleton<UsuarioService>(
    sp => new UsuarioService(sp.GetRequiredService<UsuarioDA>(),
    sp.GetRequiredService<EncryptionService>())
);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

app.Run();