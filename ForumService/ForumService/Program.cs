using DAL;
using Service.Implementation;
using Service.Interface;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITopicService, TopicService>();
builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddTransient<IFileService, FileService>();

var app = builder.Build();

app.MapControllers();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
