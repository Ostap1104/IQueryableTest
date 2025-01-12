
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QueryableCore.RepositoriesInterfaces;
using QueryableCore.Services;
using QueryableCore.Services.Interfaces;
using QueryableDatabase.Mapping;
using QueryableDatabase.Migrations;
using QueryableDatabase.Repositories;

namespace QueryableAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<MsSqlContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlContext"));
            });

            #region Dependency Injection
            builder.Services.AddTransient<IBuildingsRepository, BuildingsRepository>();
            builder.Services.AddTransient<IBuildingsService, BuildingsService>();
            #endregion

            var config = new MapperConfiguration(c => {
                c.AddProfile<QueryableDatabaseMapperProfile>();
            });
            //config.AssertConfigurationIsValid();
            builder.Services.AddSingleton<IMapper>(s => config.CreateMapper());
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
