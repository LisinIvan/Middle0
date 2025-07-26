using Microsoft.EntityFrameworkCore;
using Middle0.Application.Service;
using Middle0.Application.Service.Interfaces;
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

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

//---------------------------------------------------------------------------------

Log.Logger = new LoggerConfiguration()
	.WriteTo.MongoDB(
		databaseUrl: "mongodb://localhost:8080/logs",   // ������ ����������� � ��
		collectionName: "logevents"                      // ��� ���������
	)
	.Enrich.FromLogContext()
	.Enrich.WithProperty("Application", "MyApp")          // �������������� ���������� ���������
	.CreateLogger();

builder.Host.UseSerilog();

app.MapGet("/", () =>
{
	Log.Information("��� �������������� ���������");      // ������ ����
	return "Hello World!";
});

//--------------------------------------------------------------------------------

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
