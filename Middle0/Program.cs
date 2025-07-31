using Microsoft.EntityFrameworkCore;
using Middle0.Application.Service;
using Middle0.Application.Service.Interfaces;
using Middle0.Middlewares;
using Middle0.Persistence.Context;
using Middle0.Persistence.Repositories;
using Middle0.Persistence.Repositories.Interfaces;
using Middle0.Persistence.SeedData;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: MyAllowSpecificOrigins,
		policy =>
		{
			policy.WithOrigins(
				apiBaseUrl!
				)
			.AllowAnyHeader()
			.AllowAnyMethod();
		});
});

builder.Services.AddDbContext<EventDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionDocker")));

//---------------------------------------------------------------------------------

Log.Logger = new LoggerConfiguration()
	.WriteTo.MongoDB(
		databaseUrl: "mongodb://localhost:8080/logs",   // строка подключения и БД
		collectionName: "logevents"                      // имя коллекции
	)
	.Enrich.FromLogContext()
	.Enrich.WithProperty("Application", "MyApp")          // необязательное добавление контекста
	.CreateLogger();

builder.Host.UseSerilog();



//--------------------------------------------------------------------------------

var app = builder.Build();

//----------------------------------------------------------

app.UseMiddleware<ErrorHandlingMiddleware>();

// Тестовый маршрут
app.MapGet("/", () =>
{
	// Искусственная ошибка
	throw new Exception("Тестовая ошибка");
});

/*app.MapGet("/", () =>
{
	Log.Information("Это информационное сообщение");      // пример лога
	return "Hello World!";
});*/
//----------------------------------------------------------

app.UseCors(MyAllowSpecificOrigins);



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<EventDbContext>();
	db.Database.Migrate();
	DbInitializer.Seed(db);
}

app.Run();
