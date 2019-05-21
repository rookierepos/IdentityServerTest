using ClientApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ClientApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Global.Config.IdentityMode == Global.IdentityMode.OriginalIdentity)
            {
                services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
                var jwtSettings = new JwtSettings();
                Configuration.Bind("JwtSettings", jwtSettings);
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    #region original jwt

                    //options.TokenValidationParameters = new TokenValidationParameters
                    //{
                    //    ValidIssuer = jwtSettings.Issuer,
                    //    ValidAudience = jwtSettings.Audience,
                    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    //};

                    #endregion

                    #region costom jwt

                    options.SecurityTokenValidators.Clear();

                    options.SecurityTokenValidators.Add(new Config.CostomTokenValidator());
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Headers["expcat_token"];
                            context.Token = token.Count > 0 ? token[0] : null;
                            return Task.CompletedTask;
                        }
                    };

                    #endregion
                });

                #region authorize policy

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("admin", policy => policy.RequireClaim("AdminOnly"));
                });

                #endregion

            }
            else if (Global.Config.IdentityMode == Global.IdentityMode.ClientCredentials
                || Global.Config.IdentityMode == Global.IdentityMode.OwnerPassword)
            {
                services.AddAuthentication("Bearer")
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = "http://localhost:5000";
                        options.RequireHttpsMetadata = false;
                        options.ApiName = "api1";
                    });
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (Global.Config.IdentityMode == Global.IdentityMode.OriginalIdentity)
            {

            }
            else
            {

            }
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
