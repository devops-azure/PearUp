using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PearUp.ApplicationServices;
using PearUp.Authentication;
using PearUp.Business;
using PearUp.DomainServices;
using PearUp.Factories.Implementation;
using PearUp.Factories.Interfaces;
using PearUp.IApplicationServices;
using PearUp.IBusiness;
using PearUp.IDomainServices;
using PearUp.Infrastructure;
using PearUp.Infrastructure.Azure;
using PearUp.IRepository;
using PearUp.MongoRepository;
using PearUp.Repository;
using PearUp.Utilities;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;

namespace PearUp.ServiceConfiguration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterPearUpServices(services);
            RegisterPearUpRepositories(services);
            RegisterPhoneVerificationService(services, configuration);
            RegisterFactories(services);
            return services;
        }

        private static void RegisterFactories(IServiceCollection services)
        {
            services.AddScoped<IUserFactory, UserFactory>();
        }

        private static void RegisterPearUpRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMongoUserRepository, MongoUserRepository>();
            services.AddScoped<IInterestRepository, InterestRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
        }

        private static void RegisterPearUpServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<HttpClient>();
            services.AddSingleton<HashAlgorithm, SHA256CryptoServiceProvider>();
            services.AddSingleton<RandomNumberGenerator, RNGCryptoServiceProvider>();
            services.AddSingleton<IHashingService, HashingService>();
            services.AddScoped<IPearUpAuthenticationService, PearUpAuthenticationService>();
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddSingleton<IRandomNumberProvider, RandomNumberProvider>();
            services.AddScoped<IVerificationCodeDataStore, VerificationCodeDataStore>();
            services.AddScoped<SmtpClient, SmtpClient>();
            services.AddSingleton<PearUpMongoContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IIdentityProvider, IdentityProvider>();
            services.AddScoped<IUserDomainService, UserDomainService>();
            services.AddScoped<IInterestService, InterestService>();
            services.AddScoped<IAzureSasProvider, AzureSasProvider>();
            services.AddSingleton<IContainerFactory, ContainerFactory>();
        }

        private static void RegisterPhoneVerificationService(IServiceCollection services, IConfiguration configuration)
        {
            var type = configuration.GetValue<int>("PhoneVerificationType");
            if (type == 1)
            {
                services.AddScoped<IPhoneVerificationService, TwilioVerificationService>();
            }
            else
            {
                services.AddScoped<IPhoneVerificationService, EmailVerificationService>();
            }
        }
    }
}