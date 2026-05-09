using HRMS.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ── Service Registration (DI Container) ─────────────────
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
