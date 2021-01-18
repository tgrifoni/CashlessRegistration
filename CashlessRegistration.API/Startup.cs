using AutoMapper;
using CashlessRegistration.API.Domain.Queries.v1;
using CashlessRegistration.API.IoC;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Text.Json.Serialization;

namespace CashlessRegistration.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });
            services.AddAutoMapper(typeof(Startup))
                .AddSwaggerGen(setup =>
                {
                    setup.SwaggerDoc(Configuration["SwaggerGen:Name"], new OpenApiInfo
                    {
                        Title = Configuration["SwaggerGen:OpenApiInfo:Title"],
                        Version = Configuration["SwaggerGen:OpenApiInfo:Version"],
                        Description = Configuration["SwaggerGen:OpenApiInfo:Description"]
                    });
                    setup.AddSecurityDefinition(Configuration["SwaggerGen:OpenApiSecurityScheme:Scheme"], new OpenApiSecurityScheme
                    {
                        Name = Configuration["SwaggerGen:OpenApiSecurityScheme:Name"],
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = Configuration["SwaggerGen:OpenApiSecurityScheme:Scheme"],
                        BearerFormat = Configuration["SwaggerGen:OpenApiSecurityScheme:BearerFormat"],
                        In = ParameterLocation.Header,
                        Description = Configuration["SwaggerGen:OpenApiSecurityScheme:Description"]
                    });
                    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = Configuration["SwaggerGen:OpenApiSecurityScheme:Scheme"]
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                })
                .AddMediatR(typeof(AbstractQuery<>))
                .AddServices()
                .AddRepositories()
                .AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger()
                .UseSwaggerUI(c => c.SwaggerEndpoint(Configuration["SwaggerEndpoint:Url"], Configuration["SwaggerEndpoint:Name"]))
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHealthChecks("/health");
                });
        }
    }
}
