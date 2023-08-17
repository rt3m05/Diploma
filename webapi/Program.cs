using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using webapi.Auth;
using webapi.DB;
using webapi.DB.Repositories;
using webapi.DB.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//Configure Auth settings
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));

//Add authentication with JWT
var authSettings = builder.Configuration.GetSection("AuthSettings").Get<AuthSettings>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidIssuer = authSettings!.ISSUER,
                       ValidateAudience = true,
                       ValidAudience = authSettings.AUDIENCE,
                       ValidateLifetime = true,
                       IssuerSigningKey = authSettings.GetSymmetricSecurityKey(),
                       ValidateIssuerSigningKey = true,
                   };
               });
builder.Services.AddAuthorization();

//Add Auth service
builder.Services.AddScoped<IAuthService, AuthService>();

//Configure DB settings
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));

//Add DB context
builder.Services.AddSingleton<DataContext>();
//Add tables services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITabRepository, TabRepository>();
builder.Services.AddScoped<ITabService, TabService>();
builder.Services.AddScoped<ITileRepository, TileRepository>();
builder.Services.AddScoped<ITileService, TileService>();
builder.Services.AddScoped<ITileItemRepository, TileItemRepository>();
builder.Services.AddScoped<ITileItemService, TileItemService>();

var app = builder.Build();

//Create DB and tables if they don`t exists
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Init();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//CORS
//app.UseCors(x => x
//        .AllowAnyOrigin()
//        .AllowAnyMethod()
//        .AllowAnyHeader());

app.Run();
