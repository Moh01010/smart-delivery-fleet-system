
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Smart_Delivery___Fleet_Management_System.Data;
using Smart_Delivery___Fleet_Management_System.Data.Seeder;
using Smart_Delivery___Fleet_Management_System.Hubs;
using Smart_Delivery___Fleet_Management_System.Interface;
using Smart_Delivery___Fleet_Management_System.Repository;
using Smart_Delivery___Fleet_Management_System.Services;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Delivery___Fleet_Management_System
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //////////
            builder.Services.AddDbContext<DeliveryDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("CS")
                )
            );

            ////
            builder.Services.AddScoped<IDriversRepository, DriversRepository>();
            builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
            builder.Services.AddScoped<IDriverOrdersService, DriverOrdersService>();
            builder.Services.AddScoped<IDispatcherService, DispatcherService>();
            builder.Services.AddScoped<IReportsService, ReportsService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<JwtService>();
            builder.Services.AddScoped<PasswordHasher>();
            builder.Services.AddSignalR();

            //builder.Services.AddHttpClient<GoogleMapsService>();
            builder.Services.AddHttpClient<OpenStreetMapService>();

            builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
        };
    });
            //////
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] {}
                    }
                });
            });

            /////
            builder.Services.AddAuthorization();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();
                var hasher = scope.ServiceProvider.GetRequiredService<PasswordHasher>();

                await IdentitySeeder.SeedAsync(context, hasher);
            }
            app.MapHub<TrackingHub>("/trackingHub");


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
