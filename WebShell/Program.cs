using Microsoft.EntityFrameworkCore;
using WebShell.Connection;
using WebShell.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurações de SSH são lidas do appsettings.json
builder.Services.Configure<SshSettings>(builder.Configuration.GetSection("SshSettings"));

// Adiciona o SshService como um serviço Scoped
builder.Services.AddScoped<SshService>(); // Mantemos como Scoped

// Configurações padrão do ASP.NET Core MVC
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
    // O valor padrão do HSTS é de 30 dias. Você pode querer mudá-lo para cenários de produção.
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
