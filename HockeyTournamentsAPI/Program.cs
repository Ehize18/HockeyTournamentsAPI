using HockeyTournamentsAPI;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

config.AddEnvironmentVariables();

builder.Services.AddApplicationCors();

// Add services to the container.
builder.Services
    .AddPostgreSQLDb(config)
    .AddDbRepositories();
builder.Services.AddJwtAuthentication(config.GetSection("Jwt"));
builder.Services.AddApplicationServices();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "HockeyTournamentsAPI.xml");
    c.IncludeXmlComments(filePath);
});

builder.Services.AddBackGroundServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDevPolicy();
}

app.MigrateDb();

await app.CheckDefaultUsers();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
