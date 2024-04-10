using DAL;
using Domain.Entity;
using ForumService.DataSeed;
using ForumService.Extensions;
using ForumService.Handlers;
using ForumService.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service.Implementation;
using Service.Interface;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection"))
);
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITopicService, TopicService>();
builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddWebSocketsManager();

//builder.Services.AddHostedService<WorkerService>();
//builder.Services.AddSingleton<DataSeeder>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = false,
			ValidateLifetime = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
		};
	});

var app = builder.Build();

app.MigrateDatabase();

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();
app.UseMiddleware<WebSocketsMiddleware>();

app.MapControllers();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
