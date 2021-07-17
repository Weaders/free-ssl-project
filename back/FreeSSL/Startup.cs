using FreeSSL.Domain.Exceptions;
using FreeSSL.Domain.Options;
using FreeSSL.Domain.SSLService;
using FreeSSL.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Options;

namespace FreeSSL
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = new ConfigurationBuilder()
				.AddConfiguration(configuration)
				.AddJsonFile("appsettings.User.json", true)
				.Build();
		}

		public IConfiguration Configuration { get; }
		
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<ISSLCtrlService, SSLCtrlService>();

			services.AddCors(opts =>
			{
				opts.AddDefaultPolicy(build =>
				{
					build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
				});
			});
			
			services.AddControllers()
				.AddNewtonsoftJson();

			services.AddMemoryCache();
			services.AddHttpClient();

			services.Configure<AccountDataOptions>(Configuration.GetSection("AccountData"));
			
		}
		
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<AccountDataOptions> opts)
		{
			
			Debug.Assert(!string.IsNullOrWhiteSpace(opts.Value.Email));
			
			app.UseCors();
			
			app.UseExceptionHandler(err =>
				err.Run(async ctx =>
				{
					var feature = ctx.Features.Get<IExceptionHandlerPathFeature>();

					ctx.Response.StatusCode = 500;

					if (feature.Error is IWithHumanOutput humanOutput)
					{
						await ctx.Response.WriteAsJsonAsync(new ObjectError(humanOutput));
					}
					else
					{
						await ctx.Response.WriteAsJsonAsync(new ObjectError(feature.Error));
					}
				})
			);
			
#if !DEBUG
			app.UseHttpsRedirection();
#endif
			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.Map("/info", async context =>
				{

					context.Response.ContentType = "text/plain";

					// Host info
					var name = Dns.GetHostName(); // get container id
					var ip = Dns.GetHostEntry(name).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
					Console.WriteLine($"Host Name: { Environment.MachineName} \t {name}\t {ip}");
					await context.Response.WriteAsync($"Host Name: {Environment.MachineName}{Environment.NewLine}");
					await context.Response.WriteAsync(Environment.NewLine);

					// Request method, scheme, and path
					await context.Response.WriteAsync($"Request Method: {context.Request.Method}{Environment.NewLine}");
					await context.Response.WriteAsync($"Request Scheme: {context.Request.Scheme}{Environment.NewLine}");
					await context.Response.WriteAsync($"Request URL: {context.Request.GetDisplayUrl()}{Environment.NewLine}");
					await context.Response.WriteAsync($"Request Path: {context.Request.Path}{Environment.NewLine}");

					// Headers
					await context.Response.WriteAsync($"Request Headers:{Environment.NewLine}");

					foreach ((string key, Microsoft.Extensions.Primitives.StringValues value) in context.Request.Headers)
					{
						await context.Response.WriteAsync($"\t {key}: {value}{Environment.NewLine}");
					}

					await context.Response.WriteAsync(Environment.NewLine);

					// Connection: RemoteIp
					await context.Response.WriteAsync($"Request Remote IP: {context.Connection.RemoteIpAddress}");

				});
			});
			
		}
	}
}
