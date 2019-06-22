using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using server.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace server
{
    public class Startup
    {
        public const string CookieAuthScheme = "CookieAuthScheme";
        public const string JWTAuthScheme = "JWTAuthScheme";

        // TODO: you want this to be part of the configuration and a real secret!
        public static readonly SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes("this would be a real secret"));


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddSession(options =>
            {
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            // Sets the default scheme to cookies
            services.AddAuthentication(CookieAuthScheme)
                // Now configure specific Cookie and JWT auth options
                .AddCookie(CookieAuthScheme, options =>
                {
                    // Change the cookie name and more importantly
                    options.Cookie.Name = "soSignalRAuthCookie";
                    // Set the samesite cookie parameter as none, otherwise a scenario where the client is on a different domain wont work!
                    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                    // Simply return 401 responses when authentication fails (as opposed to default redirecting behaviour)
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = redirectContext =>
                        {
                            redirectContext.HttpContext.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                    };
                    // Finally, add an auth scheme selector that chooses the JWT scheme
                    // when connecting to SignalR's hub using the /question-hub-jwt path
                    options.ForwardDefaultSelector = ctx =>
                    {

                        return ctx.Request.Path.StartsWithSegments("/question-hub-jwt") ? JWTAuthScheme : CookieAuthScheme;
                    };
                    // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JWTAuthScheme, options =>
                {
                    // Configure JWT Bearer Auth to expect our security key
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            LifetimeValidator = (before, expires, token, param) =>
                            {
                                return expires > DateTime.UtcNow;
                            },
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateActor = false,
                            ValidateLifetime = true,
                            IssuerSigningKey = SecurityKey,
                        };

                    // We have to hook the OnMessageReceived event in order to
                    // allow the JWT authentication handler to read the access
                    // token from the query string when a WebSocket or
                    // Server-Sent Events request comes in.
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["jwt_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/question-hub-jwt")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            // Tells SignalR how to get the User unique Id from the ClaimsPrincipal
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable CORS so the Vue client can send requests
            app.UseCors(builder =>
                builder
                    .WithOrigins("http://localhost:8080")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );

            app.UseAuthentication();

            // Register SignalR hubs
            app.UseSignalR(route =>
            {
                route.MapHub<QuestionHub>("/question-hub");
                route.MapHub<QuestionHub>("/question-hub-jwt");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
