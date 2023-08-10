using webapi.DB;
using webapi.DB.Repositories;
using webapi.DB.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure DB settings
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));

//Add DB context
builder.Services.AddSingleton<DataContext>();
//Add tables services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

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

app.UseAuthorization();

app.MapControllers();

//CORS
//app.UseCors(x => x
//        .AllowAnyOrigin()
//        .AllowAnyMethod()
//        .AllowAnyHeader());

app.Run();
