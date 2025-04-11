using HockeyTournamentsAPI;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services
    .AddPostgreSQLDb(config.GetConnectionString("PostgreSQL")!)
    .AddDbRepositories();
builder.Services.AddJwtAuthentication(config.GetSection("Jwt"));
builder.Services.AddApplicationServices();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.CheckDefaultRoles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
