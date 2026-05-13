using E_Commerce.API.Extensions;
using E_Commerce.API.Middlewares;
using Service.MappingProfiles;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddCoreServices(builder.Configuration);
            builder.Services.AddPresentationServices();
            builder.Services.AddTransient<PictureUrlResolver>();
        
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
      
            var app = builder.Build();
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            await app.SeedDbAsync();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(); 
            }
          
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        
    }
}
