using VenhanBookManagementTask.Services;
using Microsoft.EntityFrameworkCore;
using VenhanBookManagementTask.DAL;
using VenhanBookManagementTask.Middleware;
using VenhanBookManagementTask.Repository;
using VenhanBookManagementTask.Repository.Interfaces;
using VenhanBookManagementTask.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ? Database connection
builder.Services.AddDbContext<BookContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDb")));

// ? Add CORS policy to allow only React frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ? Add services
builder.Services.AddControllers();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowerRepository, BorrowerRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBorrowerService, BorrowerService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ? Enable CORS **before** HTTPS redirection
app.UseCors("AllowReactFrontend");

// Middleware for exception handling
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();
