using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Writed.Models;
using Writed.Data;
using Writed.Policies.Requirements;
using Writed.Policies.Handlers;
using Writed.Services.Interfaces;
using Writed.Services.Implementations;

namespace Writed
{
    public class Program
    {
        private const int MinimumAge = 18;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseContext") ?? throw new InvalidOperationException("Connection string 'BookStoreContext' not found.")));

            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationContext>();



            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("IsAnAdult", policyBuilder =>
                {
                        policyBuilder.RequireAuthenticatedUser();
                        policyBuilder.AddRequirements(
                        new MinimumAgeRequirement(MinimumAge));
                });

                options.AddPolicy("CanManage", policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsAuthorRequirement());
                });
            });

            builder.Services.AddScoped<ICommunityService, CommunityService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ICommentService, CommentService>();

            builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, IsAuthorHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Shared/NotFound");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapRazorPages();

            app.Run();
        }
    }
}