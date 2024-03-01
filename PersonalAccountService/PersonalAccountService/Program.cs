using BusinessLogic.Implementation;
using BusinessLogic.Interface;
using DAL.DbContext;
using DAL.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using PersonalAccountService.Middleware;
using System;
using System.Text;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
	var builder = WebApplication.CreateBuilder(args);

	builder.Services.AddControllers();
	builder.Services.AddEndpointsApiExplorer();

	builder.Services.AddSingleton<ApplicationDbContext>(provider =>
	{
		var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");
		var dbName = builder.Configuration["Database:Name"];

		return new ApplicationDbContext(connectionString ?? throw new ArgumentNullException(),
										dbName ?? throw new ArgumentNullException());
	});
	builder.Services.AddTransient<IUserService, UserService>();
	builder.Services.AddTransient<IUserDataService, UserDataService>();
	builder.Services.AddTransient<ILoginService, LoginService>();

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

	builder.Logging.ClearProviders();
	builder.Host.UseNLog();

	var app = builder.Build();

	//app.UseMiddleware<JwtAuthentication>();
	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllers();
	app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");

	app.Run();
}
catch (Exception ex)
{
	logger.Error(ex.Message, "Stopped program because of exception");
	throw;
}
finally
{
	NLog.LogManager.Shutdown();
}
