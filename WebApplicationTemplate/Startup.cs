using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using WebApplicationTemplate.Data;
using WebApplicationTemplate.Models;
using WebApplicationTemplate.Security;

namespace WebApplicationTemplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {

           // Configuration = configuration;

            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

           

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configurando o acesso a dados de produtos
            //services.AddDbContext<CatalogoDbContext>(options =>
            //    options.UseInMemoryDatabase("InMemoryDatabase"));
            //services.AddScoped<ProdutoService>();

            // Configurando o uso da classe de contexto para
            // acesso às tabelas do ASP.NET Identity Core
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("IdentityDatabase"));
            //options.UseSqlServer(Configuration.GetConnectionString("IdentityDatabase")));

            // Ativando a utilização do ASP.NET Identity, a fim de
            // permitir a recuperação de seus objetos via injeção de
            // dependências
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configurando a dependência para a classe de validação
            // de credenciais e geração de tokens
            services.AddScoped<AccessManager>();

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            // Aciona a extensão que irá configurar o uso de
            // autenticação e autorização via tokens
            services.AddJwtSecurity(signingConfigurations, tokenConfigurations);

            services.AddCors();

            services.AddControllers();
           
            //services.AddMvc(c =>
            //{
            //    c.EnableEndpointRouting = false;

            //}).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info
            //    {
            //        Version = "v1",
            //        Title = "API",
            //        Description = "Test API with ASP.NET Core 3.0",
            //        TermsOfService = "None",
            //        Contact = new Contact()
            //        {
            //            Name = "Dotnet Detail",
            //            Email = "dotnetdetail@gmail.com",
            //            Url = "www.dotnetdetail.net"
            //        },
            //        License = new License
            //        {
            //            Name = "ABC",
            //            Url = "www.dotnetdetail.net"
            //        },
            //    });

            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            new IdentityInitializer(context, userManager, roleManager)
               .Initialize();

            //app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseMvc();
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    //c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            //});
        }
    }
}
