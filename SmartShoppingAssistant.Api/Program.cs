using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using OpenAI;
using SmartShoppingAssistant.BussinesLogic.Agents;
using SmartShoppingAssistant.BussinesLogic.Services;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("SmartShoppingAssistantContext");

builder.Services.AddDbContext<SmartShoppingAssistantDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<ICartItemsRepository, CartItemsRepository>();

builder.Services.AddScoped<IRepository<Product>, BaseRepository<Product>>();
builder.Services.AddScoped<IRepository<Category>, BaseRepository<Category>>();
builder.Services.AddScoped<IRepository<Promotion>, BaseRepository<Promotion>>();
builder.Services.AddScoped<IRepository<CartItems>, BaseRepository<CartItems>>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<ICartItemsService, CartItemsService>();

var openAiApiKey = builder.Configuration["OpenAI:ApiKey"]
    ?? throw new InvalidOperationException("OpenAI API key is not configured.");
var openAiModel = builder.Configuration["OpenAI:Model"] ?? "gpt-4o";

builder.Services.AddSingleton<IChatClient>(
    new OpenAIClient(openAiApiKey)
        .GetChatClient(openAiModel)
        .AsIChatClient()
        .AsBuilder()
        .UseFunctionInvocation()
        .Build());

builder.Services.AddScoped<IPromotionCheckerAgent, PromotionCheckerAgent>();
builder.Services.AddScoped<ISuggestionComposer, SuggestionComposerAgent>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "SmartShoppingAssistant API v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowAnyOrigin");
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SmartShoppingAssistantDbContext>();
        
        await DbInitializer.SeedAsync(context);
    }
    catch (Exception ex)
    {
        
        Console.WriteLine($"A aparut o eroare la seeding: {ex.Message}");
    }
}

app.MapControllers();

app.Run();
