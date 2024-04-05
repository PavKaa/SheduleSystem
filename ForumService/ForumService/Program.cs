using DAL;
using Domain.Entity;
using ForumService.DataSeed;
using ForumService.Extensions;
using ForumService.Handlers;
using ForumService.Middlewares;
using Service.Implementation;
using Service.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITopicService, TopicService>();
builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddWebSocketsManager();

builder.Services.AddHostedService<WorkerService>();
//builder.Services.AddSingleton<DataSeeder>();

var app = builder.Build();

//var databaseInitializer = app.Services.GetRequiredService<DataSeeder>();

app.UseWebSockets();
app.UseMiddleware<WebSocketsMiddleware>();

app.MapControllers();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
