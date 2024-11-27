using Microsoft.OpenApi.Models;
using __ProjectName__.Application;
using __ProjectName__.Infrastructure;
using __ProjectName__.Persistence;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();


builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
        {
            Title = "WebApplication",
            Version = "v1",
            Description = "A simple web application, which user can manage tasks",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "Karol",
                Email = String.Empty,
                Url = new Uri("https://example.com/me"),
            },
            License = new OpenApiLicense
            {
                Name = "Name license",
                Url = new Uri("https://example.com/license"),
            }
        });
        var filePath = Path.Combine(AppContext.BaseDirectory, "{{ProjectName}}.Api.xml");
        c.IncludeXmlComments(filePath);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureMiddlewares();

app.UseSession();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/home", () => "/swagger/index.html");

app.Run();