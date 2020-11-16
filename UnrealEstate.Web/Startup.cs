using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using UnrealEstate.Data.EF;
using UnrealEstate.Data.Entities;
using UnrealEstate.Mapper;
using UnrealEstate.Repository.Wrapper;
using UnrealEstate.Utilities.ActionFilter;
using UnrealEstate.Web.Controllers.Api;
using UnrealEstate.Web.Services;
using UnrealEstate.Web.Services.Bids;
using UnrealEstate.Web.Services.Comments;
using UnrealEstate.Web.Services.Common;
using UnrealEstate.Web.Services.Listings;
using UnrealEstate.Web.Services.SendMail;
using UnrealEstate.Web.Services.Users;

namespace UnrealEstate.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UnrealEstateProfile));

            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));

            services.AddSingleton<IMailer, Mailer>();

            services.AddHttpClient();


            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UnrealEstate", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
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

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //        .AddCookie(options =>
            //        {
            //            options.LoginPath = "/Login/Index";
            //            options.AccessDeniedPath = "/User/Forbidden/";
            //        });
            //        .AddGoogle(option =>
            //        {
            //            option.ClientId = "266745939810-r1avt4lufh461ht4alg6h7tqeeu5n1ra.apps.googleusercontent.com";
            //            option.ClientSecret = "EQKBxqHEf2GASy1-E0e0aOlp";
            //        });

            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddGoogle(option =>
                {
                    option.ClientId = "266745939810-r1avt4lufh461ht4alg6h7tqeeu5n1ra.apps.googleusercontent.com";
                    option.ClientSecret = "EQKBxqHEf2GASy1-E0e0aOlp";
                })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            services.AddIdentity<User, Role>()
                       .AddEntityFrameworkStores<UnrealEstateDbContext>()
                       .AddDefaultTokenProviders();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(3);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IOTimeout = TimeSpan.FromHours(3);
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllersWithViews();

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            //Declare DI
            services.AddTransient<IStorageService, FileStorageService>();
            services.AddTransient<IBidService, BidService>();
            services.AddTransient<ICommentServices, CommentServices>();
            services.AddTransient<IListingService, ListingService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<UserManager<User>, UserManager<User>>();
            services.AddTransient<SignInManager<User>, SignInManager<User>>();
            services.AddTransient<RoleManager<Role>, RoleManager<Role>>();

            services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();

            services.AddScoped<ValidationFilterAttribute>();

            services.AddTransient<ListingsController>();
            services.AddTransient<UsersController>();
            services.AddTransient<AuthController>();
            services.AddTransient<CommentsController>();
            services.AddControllersWithViews();

            // Add DbContext to the injection container
            services.AddDbContext<UnrealEstateDbContext>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //Dùng fix lỗi không nhận Url trong controller API
            services.AddScoped<IUrlHelper>(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<UnrealEstateDbContext>();
                    context.Database.EnsureCreated();
                }
            }
            else
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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger UnrealEstate Demo V1");
            });


            app.UseSession();

            var options = new RewriteOptions()
            .AddRewrite(@"delete/(\w+)/(\d+)", "comment/delete?commentId=$1&listingId=$2",
                skipRemainingRules: false)
            .AddRewrite(@"deletephoto/(\w+)/(\d+)", "listingdetail/deletephoto?listingId=$1&photoId=$2",
                skipRemainingRules: false);

            app.UseRewriter(options);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Listing}/{action=Index}/{id?}");
            });
        }
    }
}
