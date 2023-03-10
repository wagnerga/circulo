using System.Runtime.InteropServices;
using API;

Action<ILoggingBuilder> ConfigureLogging()
{
	return logger =>
	{
		logger.ClearProviders();
		logger.AddConsole();
		logger.AddLog4Net("log4net.config");
	};
}

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
	var host = Host.CreateDefaultBuilder(args)
		.ConfigureLogging(ConfigureLogging())
		.ConfigureAppConfiguration((hostContext, builder) =>
		{
			var env = hostContext.HostingEnvironment;

			builder.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json");

			builder.AddEnvironmentVariables();
		})
		.ConfigureWebHostDefaults(webBuilder =>
		{
			webBuilder.UseStartup<Startup>();
		})
		.Build();

	await host.RunAsync();
}
else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
	var host = Host.CreateDefaultBuilder(args)
		.UseSystemd()
		.ConfigureLogging(ConfigureLogging())
		.ConfigureAppConfiguration((hostContext, builder) =>
		{
			var env = hostContext.HostingEnvironment;

			builder.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json");

			builder.AddEnvironmentVariables();
		})
		.ConfigureWebHostDefaults(webBuilder =>
		{
			webBuilder.UseStartup<Startup>();
		})
		.Build();

	await host.RunAsync();
}