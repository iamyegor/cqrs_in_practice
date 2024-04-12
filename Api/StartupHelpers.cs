using System.Reflection;
using Api.Utils;
using Logic.Application.Commands.Common;
using Logic.Application.Commands.Disenroll;
using Logic.Application.Commands.EditPersonalInfo;
using Logic.Application.Commands.Enroll;
using Logic.Application.Commands.Transfer;
using Logic.Application.Queries.Common;
using Logic.Application.Queries.GetStudentsList;
using Logic.Application.Utils;
using Logic.DAL;
using Logic.DAL.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Api;

public static class StartupHelpers
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Command"));
        });

        QueryDbConnectionString.Value = builder.Configuration.GetConnectionString("Query")!;

        builder.Services.AddTransient<Messages>();
        builder.Services.AddTransient<StudentRepository>();
        builder.Services.AddTransient<CourseRepository>();

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        builder.Services.AddTransient<ICommandHandler<EnrollCommand>, EnrollCommandHandler>();
        builder.Services.AddTransient<ICommandHandler<TransferCommand>, TransferCommandHandler>();
        builder.Services.AddTransient<ICommandHandler<DisenrollCommand>, DisenrollCommandHandler>();
        builder.Services.AddTransient<
            ICommandHandler<EditPersonalInfoCommand>,
            EditPersonalInfoCommandHandler
        >();
        builder.Services.AddTransient<
            IQueryHandler<GetStudentsListQuery, IReadOnlyList<StudentInDb>>,
            GetStudentsListQueryHandler
        >();

        return builder;
    }

    public static WebApplication ConfigureMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.UseMiddleware<ExceptionHandler>();

        return app;
    }

    public static async Task ResetDatabaseAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            try
            {
                DbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occured when resetting the database: {e.Message}");
            }
        }
    }
}
