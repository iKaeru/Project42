using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MemoryCardsAPI.Helpers;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Reflection;
using MemoryCardsAPI.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Models.CardItem.Repositories;
using Models.CardItem.Services;
using Models.CardsCollection.Repositories;
using Models.CardsCollection.Services;
using Models.User.Services;
using Models.Data;
using Models.User.Repositories;
using Models.Training.Services;
using Models.Training.Repositories;
using Models.Token.Repositories;
using Models.Token.Services;

namespace MemoryCardsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ICardService, CardService>();
            services.AddSingleton<ICollectionService, CollectionService>();
            services.AddSingleton<ITrainingService, TrainingService>();
            services.AddSingleton<ITokenService, TokenService>();

          //  SetUpInMemoryDataBase(services);
            SetUpPostgreDataBase(services);

            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper();

            // Inject an implementation of ISwaggerProvider 
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1",
                    new Swashbuckle.AspNetCore.Swagger.Info {Title = "MemoryCardsAPI", Version = "v1"});

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);
                
//                swagger.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
                
                // UseFullTypeNameInSchemaIds replacement for .NET Core
                swagger.CustomSchemaIds(x => x.FullName);
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            // var userId = int.Parse(context.Principal.Identity.Name);
                            var userId = Guid.Parse(context.Principal.Identity.Name);
                            var user = userService.GetByIdAsync(userId).Result;
                            if (user == null)
                            {
                                // return unauthorized if user no longer exists
                                context.Fail("Unauthorized");
                            }

                            return Task.CompletedTask;
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

        //    services.AddAuthentication(options =>
        //        {
        //            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //        })
        //        .AddFacebook(options =>
        //        {
        //            options.AppId = Configuration["Authentication:Facebook:AppId"];
        //            options.AppSecret = Configuration["Authentication:Facebook:AppId"];
        //        })
        //        .AddGitHub(options =>
        //        {
        //            options.ClientId = Configuration["Authentication:GitHub:ClientId"];
        //            options.ClientSecret = Configuration["Authentication:GitHub:ClientSecret"];
        //        })
        //        .AddCookie(options => {
        //            options.LoginPath = "/v1/api/register";
        //        });;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("./v1/swagger.json", "MemoryCardsAPI"); });

            // global cors policy
            app.UseCors(x => x
            .WithOrigins("http://localhost:5000", "http://localhost:3000", "http://84.201.143.41:5000", "http://84.201.143.41"
            , "https://pr42.ru", "https://localhost:443", "https://pr42.ru:443")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());


            app.UseMiddleware<AuthorizationHeader>();

            app.UseAuthentication();

            app.UseMvc();
        }

        private void SetUpInMemoryDataBase(IServiceCollection services)
        {
            services.AddSingleton<IUsersRepository, InMemoryUsersRepository>();
            services.AddSingleton<ICardsRepository, InMemoryCardsRepository>();
            services.AddSingleton<ICollectionsRepository, InMemoryCollectionsRepository>();
            services.AddSingleton<ITrainingRepository, InMemoryTrainingRepository>();
            services.AddSingleton<ITokensRepository, InMemoryTokensRepository>();

            services.AddDbContext<InMemoryContext>(x => x.UseInMemoryDatabase("TestDb"));
        }

        private void SetUpPostgreDataBase(IServiceCollection services)
        {
            services.AddSingleton<IUsersRepository, PostgreUsersRepository>();
            services.AddSingleton<ICardsRepository, PostgreCardsRepository>();
            services.AddSingleton<ICollectionsRepository, PostgreCollectionsRepository>();
            services.AddSingleton<ITrainingRepository, PostgreTrainingRepository>();
            services.AddSingleton<ITokensRepository, PostgreTokensRepository>();

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<PostgreContext>()
                // .AddDbContext<PostgreContext>( opt => opt.UseNpgsql(Configuration.GetConnectionString("localConnection")))
                .BuildServiceProvider();
                // .AddDbContext<PostgreContext>( opt => opt.UseNpgsql(Configuration.GetConnectionString("nastyaLocalConnection")))
        }
    }
}
