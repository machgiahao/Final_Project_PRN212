using BookManagement.Configurations;
using BookManagement.DataAccess.Context;
using BookManagement.DataAccess.IRepositories;
using BookManagement.DataAccess.Repositories;
using BookManagement.Mappings;
using BookManagement.Services.IServices;
using BookManagement.Services.Mappings;
using BookManagement.Services.Services;
using BookManagement.SignalR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
namespace BookManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Session configuration
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddAutoMapper(
                                typeof(MappingProfile),
                                typeof(ViewModelMappingProfile));

            // Register DbContext with DI
            builder.Services.AddDbContext<BookStoreDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreDB")));

            // Register repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();

            // Register services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.Configure<OpenAIOptions>(builder.Configuration.GetSection("OpenAI"));
            builder.Services.AddHttpClient<IChatBotService, ChatBotService>();

            //Configure Authorization
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                });

            // Configure Login google
            builder.Services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");

                    googleOptions.ClientId = googleAuthNSection["ClientId"];
                    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                    googleOptions.CallbackPath = "/login-google";

                });

            builder.Services.AddAuthorization();
            builder.Services.AddSession();
            builder.Services.AddSignalR();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.MapHub<SignalRServer>("/signalRServer");
            app.MapRazorPages();

            app.Run();
        }
    }
}
