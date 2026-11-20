var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Scalar", policy =>
        policy.WithOrigins("http://localhost:5138", "http://localhost:5069")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseRouting();
app.UseCors("Scalar");

app.MapReverseProxy();

app.Run();