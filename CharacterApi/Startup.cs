using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using CharacterApi.Models;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace CharacterApi
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
      services.AddMvc();
      //adds swagger UI
      services.AddSwaggerGen();
      //adds Travel context
      services.AddDbContext<CharacterApiContext>(opt =>
        opt.UseMySql(Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(Configuration["ConnectionStrings:DefaultConnection"])));
      SetupJWTServices(services);//adds JWT

      //this adds the user context
      services.AddDbContext<CharacterApiContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));  
      services.AddControllers();

      // adds Identity
      services.AddIdentity<ApplicationUser, IdentityRole>()  
        .AddEntityFrameworkStores<CharacterApiContext>()  
        .AddDefaultTokenProviders();

      // //adds Authentication with User and JWT tokens    
      // services.AddAuthentication(options =>  
      // {  
      //   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  
      //   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  
      //   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;  
      // })
      // // adds Jwt Bearer
      // .AddJwtBearer(options =>  
      // {  
      //   options.SaveToken = true;  
      //   options.RequireHttpsMetadata = false;  
      //   options.TokenValidationParameters = new TokenValidationParameters()  
      //   {  
      //     ValidateIssuer = true,  
      //     ValidateAudience = true,  
      //     ValidAudience = Configuration["JWT:ValidAudience"],  
      //     ValidIssuer = Configuration["JWT:ValidIssuer"],  
      //     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))  
      //   };  
      // });

      //added w/ Identity/JWT etc.
      services.AddControllers().AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
      );
    }

    //JWT setup
    private void SetupJWTServices(IServiceCollection services)
    {
      string key = "password"; //this should be same which is used while creating token
      var issuer = "http://localhost:5004";  //this should be same which is used while creating token

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = issuer,
          ValidAudience = issuer,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };

        options.Events = new JwtBearerEvents
        {
          OnAuthenticationFailed = context =>
          {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
              context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
          }
        };
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseSwagger();
      app.UseSwaggerUI(c => {c.RoutePrefix = "swagger"; c.SwaggerEndpoint("/swagger/v1/swagger.json", "test");} );

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      
      // app.UseHttpsRedirection();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}