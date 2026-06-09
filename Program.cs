using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


var stripeKey =
 Environment.GetEnvironmentVariable("Stripe__Stripe_SecretKey")
	?? Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");


if (string.IsNullOrEmpty(stripeKey))
{
	var error = "Stripe Secret Key not found. Checked: appsettings.json, Stripe_SecretKey, STRIPE_SECRET_KEY, Stripe__SecretKey";
	Console.WriteLine(error);
	throw new InvalidOperationException(error);
}

Console.WriteLine($"Stripe key loaded (starts with: {stripeKey.Substring(0, Math.Min(10, stripeKey.Length))}...)");
// Add this BEFORE StripeConfiguration.ApiKey
Console.WriteLine($"Raw key from config: '{stripeKey}'");
Console.WriteLine($"Key length: {stripeKey.Length}");
Console.WriteLine($"Key starts with sk_test_: {stripeKey.StartsWith("sk_test_")}");
Console.WriteLine($"Contains spaces: {stripeKey.Contains(" ")}");
Console.WriteLine($"Contains newline: {stripeKey.Contains("\n")}");

Console.WriteLine($"KEY DEBUG: [{stripeKey}]");

StripeConfiguration.ApiKey = stripeKey;

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
			.AllowAnyMethod()
			.AllowCredentials();
	});
});

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// CORS
app.UseCors("AngularApp");

app.UseAuthorization();
app.MapControllers();

app.Run();