using Contacts.Server;
using Contacts.Server.Configurations;
using Contacts.Server.Context;
using Contacts.Server.Middleware;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
    );

builder.Services
    .AddSwaggerConfiguration()
    .AddDatabaseConfiguration(builder.Configuration)
    .AddCorsConfiguration()
    .AddApplicationServices(builder.Configuration);

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
//    db.Database.Migrate();

//    var seeder = new DbSeeder(db);
//    seeder.Seed();
//}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwaggerConfiguration();

app.UseMiddleware<ErrorHandler>();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
