using FreeSSL.Domain;
using FreeSSL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace FreeSSL
{
	public class Startup
	{

		readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<ISSLCtrlService, SSLCtrlService>();
			
			services.AddControllers()
				.AddNewtonsoftJson();

			services.AddMemoryCache();

			services.Configure<AccountDataOptions>(Configuration.GetSection(AccountDataOptions.KEY_SETTING));

			services.AddCors(opts =>
			{
				opts.AddPolicy(name: MyAllowSpecificOrigins, build =>
				{
					build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
				});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseCors(MyAllowSpecificOrigins);

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
