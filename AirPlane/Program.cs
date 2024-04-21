

using AirPlane.Mapper;
using AirPlane.Repo.IReposi;
using AirPlane.Repo.Reposi;
using AirPlane.Service.IService;
using AirPlane.Service.Service;
using AirPlane.VNpay;
using Data.DBContext;
using Login.Helper;
using Login.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SendMailAndPayMent.MailService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddDbContext<MyDb>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DB")));
//builder.Services.AddStackExchangeRedisCache(x => {
//    string connection = builder.Configuration.GetConnectionString("Redis");
//    x.Configuration = connection;
//});
// Add services to the container.
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<FlightMapper>();
builder.Services.AddScoped<IAirportService, AirportService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<Token>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<VNService>();
builder.Services.AddScoped<VnPayLibrary>();
builder.Services.AddTransient<EmailService>();



//builder.Services.AddMemoryCache();
//builder.Services.AddSingleton<SeatService>();

//Repo
builder.Services.AddScoped<IFlightRepo, FlightRepo>();
builder.Services.AddScoped<IAirPortRepo, AirPortRepo>();
builder.Services.AddScoped<IPromotionRepo, PromotionRepo>();
builder.Services.AddScoped<ITicketRepo, TicketRepo>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAcessTokenRepo, AcessTokenRepo>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "AirPlane", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Token256"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
    options.AddPolicy("ManagermentPolicy", policy => policy.RequireRole("Managerment"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:54457")
            .AllowAnyHeader()
            .WithMethods("POST", "GET", "PUT", "DELETE")
            .AllowCredentials();
    });
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
