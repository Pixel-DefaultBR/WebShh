using Microsoft.EntityFrameworkCore;
using WebShell.Connection;
using WebShell.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura��es de SSH s�o lidas do appsettings.json
builder.Services.Configure<SshSettings>(builder.Configuration.GetSection("SshSettings"));

// Adiciona o SshService como um servi�o Scoped
builder.Services.AddScoped<SshService>(); // Mantemos como Scoped

// Configura��es padr�o do ASP.NET Core MVC
builder.Services.AddControllersWithViews();

try
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))));
} 
catch (Exception ex)
{
    Console.WriteLine("Erro ao conectar ao banco de dados: " + ex.Message);
}


var app = builder.Build(); // Chame Build aqui

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // O valor padr�o do HSTS � de 30 dias. Voc� pode querer mud�-lo para cen�rios de produ��o.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
