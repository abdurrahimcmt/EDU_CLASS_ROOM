using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using EDU_DataAccess.Data;
using EDU_DataAccess.Initializer;
using EDU_DataAccess.Repository;
using EDU_DataAccess.Repository.IRepository;
using EDU_Utility;
using EDU_Utility.BrainTree;
using EDU.Service;

namespace EDU
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
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders().AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.Configure<IdentityOptions>(opt =>
            {
                // Password must be greater then 5
                opt.Password.RequiredLength = 5;
                // Password must have lowercase letter
                opt.Password.RequireLowercase = true;
                // when the user goes to lockout season it will be for 30 second
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
                // User should go to lockout after 5 times fails to login
                opt.Lockout.MaxFailedAccessAttempts = 5;
                // User should must be confirm email validation.
                opt.SignIn.RequireConfirmedAccount = true;
            });
            services.ConfigureApplicationCookie(opt =>
            {
                opt.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Home/Accessdenied");
            });
            services.AddTransient<IEmailSender, MailJetEmailSender>();

            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(10);
                Options.Cookie.HttpOnly = true;
                Options.Cookie.IsEssential = true;
            });


            services.Configure<BrainTreeSetting>(Configuration.GetSection("BrainTree"));
            services.AddSingleton<IBrainTreeGate, BrainTreeGete>();

            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddAuthentication().AddFacebook(Options => {
                Options.AppId = "501587157572205";
                Options.AppSecret = "004dd702cecb320443407e736f4e3b9c";
            });


            /////////Adding Interface In Services 
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IDepartmentInfoRepository, DepartmentInfoRepository>();
            services.AddScoped<ISemesterInfoRepository, SemesterInfoRepository>();
            services.AddScoped<IBatchinfoRepository, BatchinfoRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IAdminInfoRepository, AdminInfoRepository>();
            services.AddScoped<ITeacherInfoRepository,TeacherInfoRepository>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<ICourseInfoRepository, CourseInfoRepository>();
            services.AddScoped<IApplicationTypeRepository, ApplicationTypeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInquiryHeaderRepository, InquiryHeaderRepository>();
            services.AddScoped<IInquiryDetailRepository, InquiryDetailRepository>();
            services.AddScoped<IEnrollmentInfoRepository, EnrollmentInofRepository>();
            services.AddScoped<IEnrollmentDetailsRepository, EnrollmentDetailsRepository>();
            services.AddScoped<IOnlineClassInfoRepository, OnlineClassInofRepository>();
            services.AddScoped<IOnlineClassDetailsRepository, OnlineClassDetailsRepository>();

            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();


            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            dbInitializer.Initialize();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
