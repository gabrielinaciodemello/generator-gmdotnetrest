using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using <%= ProjectName %>.Repositories;
using <%= ProjectName %>.Utils;

namespace <%= ProjectName %>
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

            services.AddSingleton<IRepository>(new MongoGeneric("mongodb://localhost:27017", "TesteDB3"));
            services.AddSingleton<ILog,DbLog>();
            services.AddSingleton<IRequestsHandler,SimpleRequestHandler>();

            var jwtToken = new JwtToken("QUdUSERGUlM1MTQzR1RTVlNVQVZKWDVZVUlPRUZTUkNURFM=", 20);
            services.AddSingleton<IBearerAuth>(jwtToken);
            services.AddAuthentication()
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtToken.GetSymmetricSecurityKey(),
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
