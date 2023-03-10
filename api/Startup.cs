using System.Diagnostics;
using System.Security.Cryptography;
using Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Services;
using Services.Interfaces;

namespace API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var issuer = Configuration["JWTConfig:Issuer"];
			var audience = Configuration["JWTConfig:Audience"];
			var publicKey = Configuration["JWTConfig:PublicKey"];
			var connectionString = Configuration["ConnectionString"];

			if (publicKey != null)
				// use rsa to create jwt
				services.AddSingleton(_ =>
				{
					var rsa = RSA.Create();
					rsa.ImportSubjectPublicKeyInfo(
						Convert.FromBase64String(publicKey),
						out var _
					);

					return new RsaSecurityKey(rsa);
				});

			// add asymmetric security
			services.AddAuthentication(o =>
				{
					o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer("Asymmetric", options =>
				{
					var rsa = GetServiceProvider(services).GetRequiredService<RsaSecurityKey>();

					options.IncludeErrorDetails = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						IssuerSigningKey = rsa,
						ValidAudience = audience,
						ValidIssuer = issuer,
						RequireSignedTokens = true,
						RequireExpirationTime = true,
						ValidateLifetime = true,
						ValidateAudience = true,
						ValidateIssuer = true,
					};
				});

			if (connectionString != null)
				services.AddDbContext<CirculoContext>(options => options.UseNpgsql(connectionString));

			// using newtonsoft for serialization ignoring self reference loop
			services.AddControllers().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			});

			if (Debugger.IsAttached)
			{
				services.AddCors(options =>
				{
					options.AddPolicy("Policy1", builder => builder
						.WithOrigins("http://localhost:3000")
						.SetIsOriginAllowedToAllowWildcardSubdomains()
						.AllowAnyHeader()
						.AllowAnyMethod());
				});
			}
			else // production
			{
				services.AddCors(options =>
				{
					options.AddPolicy("Policy1", builder => builder
						.WithOrigins("https://*.circulohealth.com", "https://localhost")
						.SetIsOriginAllowedToAllowWildcardSubdomains()
						.AllowAnyHeader()
						.AllowAnyMethod());
				});
			}

			// inject services here
			services.AddTransient<IPharmacyVisitService, PharmacyVisitService>();

			// initialize swagger with newtonsoft support
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
				c.SupportNonNullableReferenceTypes();
			});

			services.AddSwaggerGenNewtonsoftSupport();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/ErrorResponse");
				app.UseHsts();
			}

			app.UseRouting();
			app.UseCors("Policy1");

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(e =>
			{
				e.MapControllers();
				e.MapSwagger();
			});

			// enable swagger middleware
			app.UseSwagger();
		}

		ServiceProvider GetServiceProvider(IServiceCollection services)
		{
			return services.BuildServiceProvider();
		}
	}
}