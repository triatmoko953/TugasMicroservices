using Microsoft.EntityFrameworkCore;
using OrderService.AsyncDataServices;
using OrderService.Data;
using OrderService.EventProccessing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddDbContext<AppDbContext>(opt =>opt.UseInMemoryDatabase("InMem"));

// untuk memanggil dan menyingkronkan data
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IWalletRepo, WalletRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddSingleton<IEventProccessor, EventProccessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddMvc()
                .AddJsonOptions(opt =>
                 {
                     opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                 });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
