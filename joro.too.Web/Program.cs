using CloudinaryDotNet;
using joro.too.DataAccess;
using joro.too.Entities;
using joro.too.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using joro.too.Services.Services.IServices;
using joro.too.Services.Services;
using Microsoft.AspNetCore.Http.Features;

namespace joro.too.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            //dbcontext setup
            builder.Services.Configure<IISServerOptions>(options =>
            {
                // 1024MB
                options.MaxRequestBodySize = 104857600;
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                // 1024MB
                options.MultipartBodyLengthLimit = 104857600;
            });
            builder.Services.Configure<HttpRequestOptions>(options =>
            {
               // options.Timeout = TimeSpan.FromSeconds(10);
            });
            builder.Services.AddDbContext<MovieDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ArchIsAssConnection")));
            //personal services setup
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<IMediaService, MediaService>();
            builder.Services.AddScoped<IEpisodeService, EpisodeService>();
            builder.Services.AddScoped<ISeasonService, SeasonService>();
            builder.Services.AddScoped<IUserService, UserServices>();
            builder.Services.AddScoped<IActorService, ActorService>();
            
            //cloudinary setup
            builder.Services.AddScoped<CloudinaryService>();
            var CloudinarySettings = builder.Configuration.GetSection("Cloudinary").Get<CloudinarySettings>();
            var acc = new Account(CloudinarySettings.CloudName, CloudinarySettings.ApiKey,
                CloudinarySettings.ApiSecret);
            var cloudinary = new Cloudinary(acc);
            builder.Services.AddSingleton(cloudinary);
            //identity setup
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<MovieDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();
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

            app.UseAuthentication();

            app.UseAuthorization();
            
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await CreateRoles(services);
            }
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await CreateAdmin(services);
            }
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                //await CreateUser(services);
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }

        private static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task CreateAdmin(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var adminEmail = "admin@admin.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var user = new User { UserName = "admin@admin.com", Email = adminEmail, Pfp = "https://res.cloudinary.com/djubwo5uq/image/upload/v1744467542/n9kfa5wcfkpmnzti1quv.webp", RatedShows = new List<Show>(), RatedMovies = new List<Movie>() };
                var result = await userManager.CreateAsync(user, "AdminPassword123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
        public static async Task CreateUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var adminEmail = "gosho@gmail.com";
            var guyUser = await userManager.FindByEmailAsync(adminEmail);
            if (guyUser == null)
            {
                var user = new User { UserName = "gosho", Email = adminEmail, Pfp = "https://res.cloudinary.com/djubwo5uq/image/upload/v1744467542/n9kfa5wcfkpmnzti1quv.webp", RatedShows = new List<Show>(), RatedMovies = new List<Movie>() };
                var result = await userManager.CreateAsync(user, "P@ssW0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}