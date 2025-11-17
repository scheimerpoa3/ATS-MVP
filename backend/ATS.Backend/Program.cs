using ATS.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Carrega MongoSettings do appsettings.json
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));

// Injeção do repositório Mongo
builder.Services.AddSingleton<ICandidatoRepository, CandidatoRepository>();

// Controllers
builder.Services.AddControllers();

// Swagger (corrigido)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para permitir Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        p => p.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Ativa Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.MapControllers();
app.Run();
