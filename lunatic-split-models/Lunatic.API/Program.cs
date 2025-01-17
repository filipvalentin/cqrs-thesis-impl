using Lunatic.API.Interfaces;
using Lunatic.API.Services;
using Lunatic.Application;
using Lunatic.Application.Contracts.Interfaces;
using Lunatic.Application.Models;
using Lunatic.Identity;
using Lunatic.Infrastructure;
using Lunatic.Infrastructure.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options => {
	options.AddPolicy("Open", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
});
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<MLAPISettings>(builder.Configuration.GetSection("MLAPISettings"));
builder.Services.AddInfrastructureToDI(builder.Configuration);
builder.Services.AddInfrastrutureIdentityToDI(builder.Configuration);
builder.Services.AddApplicationServices();
//builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IDayConversionService, DayConversionService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
		Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(
		new OpenApiSecurityRequirement() {
					{
					  new OpenApiSecurityScheme {
						Reference = new OpenApiReference {
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						  },
						  Scheme = "oauth2",
						  Name = "Bearer",
						  In = ParameterLocation.Header,
						},
						new List<string>()
					}});

	c.SwaggerDoc("v1", new OpenApiInfo {
		Version = "v1",
		Title = "Lunatic API",
	});
});
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if(app.Environment.IsDevelopment()) {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseCors("Open");

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
