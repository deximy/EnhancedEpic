
using EnhancedEpic.Models;
using EnhancedEpic.Services;
using Microsoft.EntityFrameworkCore;

namespace EnhancedEpic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<CurrencyExchangeService>();
            builder.Services.AddSingleton<EpicdbService>();

            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlite("Datasource=app.db"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(
                x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials()
            );

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
