using KuCoinApiClient;
using KuCoinApiClient.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddTransient<ILogger<KucoinProviderService>>();
builder.Services.AddTransient<HttpService>();
builder.Services.AddTransient<OrderBookService>();
builder.Services.AddTransient<KucoinProviderService>();

builder.Services.AddSingleton<KucoinMessagingService>();
builder.Services.AddSingleton<MessagesStorage>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
