using PersonBook.Web.Data;
using PersonBook.Core.Data;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Formatting.Compact;
using PersonBook.Core.Repositories;

// Set Serilog settings
var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
    .MinimumLevel.Debug()
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure settings
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(ConnectionStrings.SectionName));
builder.Services.AddSingleton(s => s.GetRequiredService<IOptions<ConnectionStrings>>().Value);

// Register work database context (MongoDB)
// The MongoDB client has a pool of connections that are reused automatically and a single MongoDB client instance is enough even in multithreaded scenarios
// See http://mongodb.github.io/mongo-csharp-driver/2.7/getting_started/quick_tour/ (Mongo Client section)
builder.Services.AddSingleton<DataContext>(x => new DataContext(x.GetService<IOptions<ConnectionStrings>>().Value.MongoDbConnection));

// Add IPFees services
builder.Services.AddTransient<IPersonRepository, PersonRepository>();
builder.Services.AddTransient<IBookRepository, BookRepository>();

// Add logger
builder.Logging.AddSerilog(logger);
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSerilogRequestLogging();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
