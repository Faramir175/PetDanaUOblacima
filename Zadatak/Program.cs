using PetDanaUOblacima.Data;
using PetDanaUOblacima.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICanteenService, CanteenService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/setup/reset", (HttpContext context) =>
{
    try
    {
        InMemoryDbContext.ResetDatabase();
        context.Response.StatusCode = 200;
        return Results.Ok(new { message = "Database successfully reset." });
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        return Results.Problem(ex.Message);
    }
});

app.Run();
