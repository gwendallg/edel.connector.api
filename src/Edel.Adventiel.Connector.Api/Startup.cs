﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.Configuration;
using Autumn.Mvc;
using Autumn.Mvc.Data;
using Autumn.Mvc.Data.MongoDB;
using Autumn.Mvc.Data.Swagger;
using Edel.Adventiel.Connector.Api.Controllers;
using Edel.Adventiel.Connector.Api.Entities;
using Edel.Adventiel.Connector.Api.Models;
using Edel.Adventiel.Connector.Api.Models.V1;
using Edel.Adventiel.Connector.Api.Models.V1.Platforms;
using Edel.Adventiel.Connector.Api.Services;
using Edel.Adventiel.Connector.Api.Swagger;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Edel.Adventiel.Connector.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAutumn(config =>
                {
                    config
                        .PageSize(50)
                        .PageSizeFieldName("l")
                        .QueryFieldName("q")
                        .SortFieldName("s")
                        .PageNumberFieldName("o")
                        .NamingStrategy(new SnakeCaseNamingStrategy());
                })
                .AddAutumnData(config =>
                    config
                        .RepositoryControllerType(type: typeof(DefaultCrudPageableRepositortyController<,,,>))
                        .ApiVersion("v1")
                        .PluralizeController(false)
                )
                .AddAutumnMongo(config =>
                    config
                        .ConnectionString($"{_configuration[key: "ConnectionStrings:0:ConnectionString"]}")
                        .Database($"{_configuration[key: "ConnectionStrings:0:Database"]}")
                        .Convention(new CamelCaseElementNameConvention())
                )
                .AddSecurity(_configuration)
                .AddSwaggerGen(c =>
                {

                    foreach (var version in services.GetAutumnDataSettings().ApiVersions)
                    {
                        c.SwaggerDoc(version, new Info {Title = "api", Version = version});
                    }

                    c.DocumentFilter<SwaggerDocumentFilter>();
                    c.OperationFilter<DefaultSwaggerOperationFilter>();
                })
                .AddInitialization(_configuration) 
                .AddMvc();
               
           
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    foreach (var version in app.GetAutumnDataSettings().ApiVersions)
                    {
                        c.SwaggerEndpoint(string.Format("/swagger/{0}/swagger.json", version),
                            string.Format("API {0}", version));
                    }
                })
                .UseAuthentication()
                .UseMvc();
        }
    }
}