using Azure.Storage.Blobs;
using FileStorageApp.API.Extensions;
using FileStorageApp.Core.Interfaces;
using FileStorageApp.Core.Mapping;
using FileStorageApp.Infrastructure.Data;
using FileStorageApp.Infrastructure.Repositories;
using FileStorageApp.Infrastructure.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Configure Authentication
        //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer(options =>
        //    {
        //        // Configure JWT Bearer authentication
        //        // You'll need to replace these with your actual Auth0 or Firebase settings
        //        options.Authority = builder.Configuration["Authentication:Authority"];
        //        options.Audience = builder.Configuration["Authentication:Audience"];
        //    });

        // Configure Database
        builder.Services.AddDbContext<FileStorageDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

        // Add services to the container
        builder.Services.AddControllers();

        // Configure file upload
        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50 MB
        });

        builder.Services.AddSingleton(sp =>
        {
            var connectionString = builder.Configuration.GetConnectionString("AzureBlobConnection");
            var containerName = builder.Configuration["AzureBlobStorage:ContainerName"];
            return new BlobContainerClient(connectionString, containerName);
        });

        // Dependency Injections
        builder.Services.AddScoped<IFileRepository, FileRepository>();
        builder.Services.AddScoped<IFolderRepository, FolderRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddScoped<IFolderService, FolderService>();
        builder.Services.AddScoped<ISharingService, SharingService>();
        builder.Services.AddScoped<IAzureBlobService, AzureBlobService>();

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        // Swagger Configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "File Storage Service API",
                Version = "v1"
            });

            //c.OperationFilter<FileUploadOperationFilter>();

            // Configure Swagger to use JWT authentication
            //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //{
            //    Description = "JWT Authorization header using the Bearer scheme",
            //    Name = "Authorization",
            //    In = ParameterLocation.Header,
            //    Type = SecuritySchemeType.ApiKey
            //});
        });

        // Add CORS to allow file uploads from different origins
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "File Storage Service API v1"));
            app.UseDeveloperExceptionPage();
        }

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();

    }
}
