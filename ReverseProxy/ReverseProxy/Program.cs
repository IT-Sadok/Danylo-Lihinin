using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("sliding", httpcontext =>

        RateLimitPartition.GetSlidingWindowLimiter(partitionKey: httpcontext.Connection.RemoteIpAddress,
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromSeconds(10),
                SegmentsPerWindow = 5,
                QueueLimit = 0
            }));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("Scalar", policy =>
        policy.WithOrigins("http://localhost:5138", "http://localhost:5069")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("X-Served-By"));
});
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseRouting();
app.UseCors("Scalar");
app.UseRateLimiter();

app.MapReverseProxy();

app.Run();