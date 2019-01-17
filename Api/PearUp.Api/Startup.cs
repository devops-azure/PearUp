using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using PearUp.Api.Diagnostics;
using PearUp.Authentication;
using PearUp.CommonEntities;
using PearUp.CommonEntity;
using PearUp.Constants;
using PearUp.Infrastructure;
using PearUp.Infrastructure.Azure;
using PearUp.LoggingFramework;
using PearUp.Repository;
using PearUp.ServiceConfiguration;
using PearUp.Utilities;
using Swashbuckle.AspNetCore.Swagger;

namespace PearUp.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddAuthnetication(services);
            AddAuthorization(services);
            AddAutoMapper(services);
            AddMVC(services);
            AddSwagger(services);
            var connectionString = Configuration.GetValue<string>("ConnectionStrings:SQLConnectionString");
            services.AddDbContext<PearUpContext>(options => options.UseSqlServer(connectionString));
            services.Configure<TwilioNetworkCredentials>(Configuration.GetSection("TwilioNetworkCredentials"));
            services.Configure<EmailServiceConfiguration>(Configuration.GetSection("EmailServiceConfiguration"));
            services.Configure<MongoSettings>(Configuration.GetSection("MongoSettings"));
            services.Configure<AzureSettings>(Configuration.GetSection("AzureSettings"));
            var serilogConfiguration = Configuration.GetSection("Serilog").Get<SerilogConfiguration>();
            serilogConfiguration.ConnectionString = connectionString;
            services.RegisterLogger(serilogConfiguration);
            services.RegisterServices(Configuration);
            services.AddMediatR();
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "PearUp Web API",
                });
            });
        }

        private void AddMVC(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            });
        }

        private void AddAutoMapper(IServiceCollection services)
        {
            var autoMapperCofig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DTOBusinessMapperProfileConfig());
            });
            services.AddSingleton(_ => autoMapperCofig.CreateMapper());
        }

        private void AddAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthConstants.PolicyUser,
                    policy => policy.RequireClaim(AuthConstants.UserId));
                options.AddPolicy(AuthConstants.PolicyAdmin,
                    policy => policy.RequireClaim(AuthConstants.AdminId));
            });
        }

        private void AddAuthnetication(IServiceCollection services)
        {
            var settings = Configuration.GetSection("Authentication:Security");
            services.Configure<AuthSettings>(settings);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                          .AddJwtBearer(options =>
                          {
                              options.TokenValidationParameters = new TokenValidationParameters
                              {
                                  ValidateIssuer = true,
                                  ValidateAudience = true,
                                  ValidateLifetime = true,
                                  ValidateIssuerSigningKey = true,

                                  ValidIssuer = settings["CleintName"],
                                  ValidAudience = settings["CleintName"],
                                  IssuerSigningKey = TokenProvider.GenerateSecret(settings["SecretKey"])
                              };
                          });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            /*Enabling cors for specific sites */
            //app.UseCors(
            // options => options.WithOrigins(Configuration.GetValue<string>("Authentication:OrginsForCors").Split(',')).AllowAnyMethod().AllowAnyHeader()
            // );
            app.UseCors(builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
            /*End Cors */
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PearUp V1");
            });
            app.UseAuthentication();
            app.UseMiddleware<ErrorLoggingMiddleware>();
            app.UseMvc();
        }
    }
}
