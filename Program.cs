using System.Text;
using apief;
using apief.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using testProd.auth;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

string? tokenKeyString = builder.Configuration.GetSection(AuthHelp.KEY_TOKEN_KEY).Value;

if (string.IsNullOrEmpty(tokenKeyString))
{
    throw new ArgumentException("TokenKey is not configured.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddSingleton<KeyConfig>();
builder.Services.AddScoped<Crypted>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<ILog, Log>();
builder.Services.AddScoped<IIdentityUser, IdentityUser>();

builder.Services.AddScoped<IAuthHelp, AuthHelp>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPassRepository, PassRepository>();
builder.Services.AddScoped<IPassService, PassService>();

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<RequestTimingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
