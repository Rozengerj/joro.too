using CloudinaryDotNet;
using joro.too.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using joro.too.Services.Services.IServices;
using joro.too.Services.Services;
using Microsoft.AspNetCore.Http.Features;

namespace joro.too.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            //dbcontext setup
            builder.Services.Configure<IISServerOptions>(options=>
            {
                // 1024MB
                options.MaxRequestBodySize = 104857600;
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                // 1024MB
                options.MultipartBodyLengthLimit = 104857600;
            });
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ArchIsNotSoAssConnection")));
            //personal services setup
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<IMediaService, MediaService>();
            //cloudinary setup
            builder.Services.AddScoped<CloudinaryService>();
            var CloudinarySettings = builder.Configuration.GetSection("Cloudinary").Get<CloudinarySettings>();
            var acc = new Account(CloudinarySettings.CloudName, CloudinarySettings.ApiKey,
                CloudinarySettings.ApiSecret);
            var cloudinary = new Cloudinary(acc);
            builder.Services.AddSingleton(cloudinary);
            //identity setup
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<MovieDbContext>();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
