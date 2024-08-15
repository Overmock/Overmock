using DataCompany.Framework;
using FakeCompany.Api.Storage;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddScopedProxy<IDataConnection, FrameworkDataConnection>(c => {
		Stopwatch stopwatch = Stopwatch.StartNew();
		c.Invoke();
		stopwatch.Stop();
		var elapsed = stopwatch.Elapsed;
	})
	.AddTransient<EntityCollection<UserStory>, UserStoryFactory>()
	.AddTransient<IUserStoryService, UserStoryService>()
    .AddScoped<IProjectService, ProjectService>()
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
