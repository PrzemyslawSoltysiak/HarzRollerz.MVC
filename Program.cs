using HarzRollerz.MVC.Data;
using HarzRollerz.MVC.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? database = builder.Configuration.GetValue<string>("Database");
switch (database)
{
    case "SQL":
        builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging());
        break;
    default: // "InMemory"
        builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("HarzRollerz.MVC").EnableSensitiveDataLogging());
        break;
}

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

builder.Services.AddScoped<ScoresService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured creating the DB.");
    }
}

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
    pattern: "{controller=Competitions}/{action=Index}/{id?}");

app.Run();
