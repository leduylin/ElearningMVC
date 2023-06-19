using Amazon;
using Amazon.RDS.Util;
using Amazon.Runtime;

using ElearningMVC.Data;

using Microsoft.EntityFrameworkCore;

namespace ElearningMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Connect to RDS
            var awsCredentials = new BasicAWSCredentials("AKIAR4FED76S6ZSRLMGJ", "nyWqKNMpFWDonbE2lGNjYtrefz5XtQrf9J4TNq8E");
            var region = RegionEndpoint.APSoutheast1;
            var rdsEndpoint = "elearning.cncgpffk4sls.ap-southeast-1.rds.amazonaws.com";
            var databasePort = 3306;
            var databaseUsername = "duylindb";

            var authToken = RDSAuthTokenGenerator.GenerateAuthToken(awsCredentials, region, rdsEndpoint, databasePort, databaseUsername);
            var str = $"Server=elearning.cncgpffk4sls.ap-southeast-1.rds.amazonaws.com,3306; Database = attendancechecking; User ID = {databaseUsername}; Password = {authToken}";

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(str, ServerVersion.AutoDetect(str)));

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