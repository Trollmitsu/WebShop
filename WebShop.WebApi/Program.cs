using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using WebShop.WebApi.Data;
using WebShop.WebApi.Repositories;
using WebShop.WebApi.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<ShopOnlineDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
    policy.WithOrigins("http://localhost:7176", "https://localhost:7176")
        .AllowAnyMethod()
        .WithHeaders(HeaderNames.ContentType)
    );


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
