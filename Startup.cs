using System;
using System.IO;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjetoForest.Helpers;
using ProjetoForest.Interfaces;
using ProjetoForest.Models;
using ProjetoForest.Repositories;

namespace ProjetoForest
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
            services.AddControllers();

            // Configuração para conexão com o banco de dados
            services.AddDbContext<Contexto>(
                x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
            );

            // Registra o gerador Swagger, definindo 1 ou mais documentos Swagger
            services.AddSwaggerGen(c =>
            {
                // Para fins de documentação
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "ToDo API",
                        Description = "A simple example ASP.NET Core Web API",
                        TermsOfService = new Uri("https://example.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "Shayne Boyer",
                            Email = string.Empty,
                            Url = new Uri("https://twitter.com/spboyer"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Use under LICX",
                            Url = new Uri("https://example.com/license"),
                        }
                    }
                );

                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    }
                );

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement 
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    }
                );
            });

            // Configuração para usar o Identity
            // options.Password configura a criação da senha
            IdentityBuilder builder = services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<Contexto>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            // Configuração para autenticação e autorização
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
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

            // Configuração para solicitar autorização nas rotas
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            // Configuração para evitar erro de "possível ciclo de objeto que não é compatíve"
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            // Implementação de Interfaces e Repositories
            services.AddScoped<IUsuario, UsuarioRepository>();

            // Mapper para Models e DTOs
            services.AddAutoMapper(typeof(MapperProfiles));

            // Configuração de permissão Cors
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Habilite o middleware para servir o Swagger gerado como um terminal JSON.
            app.UseSwagger();

            // Habilitar middleware para servir swagger-ui (HTML, JS, CSS, etc.),
            // especificando o terminal JSON Swagger.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = string.Empty;
            });

            // Comentado para usar apenas o localhost:5000
            // app.UseHttpsRedirection();

            app.UseRouting();

            // Configuração para autenticação e autorização
            app.UseAuthentication();
            app.UseAuthorization();

            // Permissões do Cors
            app.UseCors(
                x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );

            // Configuração de caminho para arquivos ou imagens
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Resources")
                ),
                RequestPath = new PathString("/Resources")
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
