using lavacarBBL.Mapeos;
using lavacarBLL.Servicios;
using lavacarDAL.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Repositorios en memoria (listas)
builder.Services.AddSingleton<IClientesRepositorio, ClientesRepositorio>();
builder.Services.AddSingleton<IVehiculosRepositorio, VehiculosRepositorio>();
builder.Services.AddSingleton<ICitasRepositorio, CitasRepositorio>();

// Servicios BLL 
builder.Services.AddSingleton<IClientesServicio, ClientesServicio>();   
builder.Services.AddSingleton<IVehiculosServicio, VehiculosServicio>(); 
builder.Services.AddSingleton<ICitasServicio, CitasServicio>();        

// AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(MapeoClases));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Clientes",
    pattern: "{controller=Cliente}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Vehiculos",
    pattern: "{controller=Vehiculo}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Citas",
    pattern: "{controller=Cita}/{action=Index}/{id?}");

app.Run();
