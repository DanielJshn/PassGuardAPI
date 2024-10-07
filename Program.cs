using apief;
using apief.Services;
using Microsoft.EntityFrameworkCore;
using testProd.auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); 
builder.Services.AddLogging();

builder.Services.AddAutoMapper(typeof(MappingProfile)); 
builder.Services.AddScoped<ILog, Log>(); 

builder.Services.AddScoped<IAuthHelp, AuthHelp>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPassRepository, PassRepository>();
builder.Services.AddScoped<IPassService, PassService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();  

app.Run();
