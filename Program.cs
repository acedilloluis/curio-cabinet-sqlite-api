using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ClimbLogDb>(options => options.UseSqlite("Data Source=app.db"));
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

//browser ui to test api; uses openapi and scalar
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

//ensure the database and tables are created on startup
using (IServiceScope scope = app.Services.CreateScope())
{
    ClimbLogDb db = scope.ServiceProvider.GetRequiredService<ClimbLogDb>();
    db.Database.EnsureCreated();
}

RouteGroupBuilder climbLogs = app.MapGroup("/logs");

climbLogs.MapGet("/", GetAllLogs);
climbLogs.MapPost("/", CreateLog);
climbLogs.MapPut("/{id}", UpdateLog);
climbLogs.MapDelete("/{id}", DeleteLog);

app.Run();

static async Task<IResult> GetAllLogs(ClimbLogDb db)
{
    return TypedResults.Ok(await db.ClimbLogs.ToArrayAsync());
}

static async Task<IResult> CreateLog(ClimbLog log, ClimbLogDb db)
{
    db.ClimbLogs.Add(log);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/logs/{log.Title}", log);
}

static async Task<IResult> UpdateLog(int id, ClimbLog log, ClimbLogDb db)
{
    ClimbLog? logToUpdate = await db.ClimbLogs.FindAsync(id);
    if (logToUpdate is null) return TypedResults.NotFound();

    logToUpdate.Title = log.Title;
    logToUpdate.Grade = log.Grade;
    logToUpdate.Date = log.Date;
    logToUpdate.Sent = log.Sent;
    logToUpdate.Attempts = log.Attempts;
    logToUpdate.Note = log.Note;

    await db.SaveChangesAsync();

    return TypedResults.NoContent(); 
}

static async Task<IResult> DeleteLog(int id, ClimbLogDb db)
{
    ClimbLog? logToDelete = await db.ClimbLogs.FindAsync(id);

    if (logToDelete is null) return TypedResults.NotFound();

    db.ClimbLogs.Remove(logToDelete);
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}