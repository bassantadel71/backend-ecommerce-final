using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Stripe
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularApp", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "http://localhost:80",
                "http://localhost"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Auto migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// CORS (IMPORTANT ORDER)
app.UseCors("AngularApp");

app.UseAuthentication();   // (add this)
app.UseAuthorization();

app.MapControllers();

// Run on Docker port
app.Run("http://0.0.0.0:8080"); // ⚠️ recommended fix