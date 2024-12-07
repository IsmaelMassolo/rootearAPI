using Microsoft.EntityFrameworkCore;
using rootearAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<apiContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSql")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
