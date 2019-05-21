using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServerCenter.Data;
using ServerCenter.Models;

namespace ServerCenter
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
                #region use efcore

                services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                    })
                    .AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

                #endregion

                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();

                services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireDigit = false;
                });
            }
            else if (Global.Config.IdentityMode == Global.IdentityMode.ClientCredentials)
            {
                #region Original ClientCredentials

                var builder = services.AddIdentityServer()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiResources(Config.GetApis())
                    .AddInMemoryClients(Config.GetClients());

                builder.AddDeveloperSigningCredential();

                #endregion
            }
            else if (Global.Config.IdentityMode == Global.IdentityMode.OwnerPassword)
            {
                var builder = services.AddIdentityServer()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiResources(Config.GetApis())
                    .AddInMemoryClients(Config.GetClients())
                    .AddTestUsers(Config.GetTestUsers().ToList());

                builder.AddDeveloperSigningCredential();
            }
            else if (Global.Config.IdentityMode == Global.IdentityMode.AuthorizationCode)
            {
                #region use efcore

                services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                    })
                    .AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

                #endregion

                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();

                services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireDigit = false;
                });

                var builder = services.AddIdentityServer()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiResources(Config.GetApis())
                    .AddInMemoryClients(Config.GetClients())
                    .AddTestUsers(Config.GetTestUsers().ToList());

                builder.AddDeveloperSigningCredential();
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (Global.Config.IdentityMode == Global.IdentityMode.OriginalIdentity)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }
                app.UseAuthentication();
            }
            else if (Global.Config.IdentityMode == Global.IdentityMode.ClientCredentials ||
                Global.Config.IdentityMode == Global.IdentityMode.OwnerPassword)
                
            {
                app.UseIdentityServer();
            }
            else if (Global.Config.IdentityMode == Global.IdentityMode.AuthorizationCode)
            {
                app.UseAuthentication();
                app.UseIdentityServer();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
