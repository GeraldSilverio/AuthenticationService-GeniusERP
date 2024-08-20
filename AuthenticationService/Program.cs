using AuthenticationService.Api.Features.Roles.Request;
using AuthenticationService.Api.Features.Roles.Validations;
using AuthenticationService.Api.Features.User.Command;
using AuthenticationService.Api.Interfaces;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Services;
using AuthenticationService.Application.Validations;
using FluentValidation;
using MediatR;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region HttpClientFactory

        builder.Services.AddHttpClient("GoogleAuth", (sp, httpClient) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();

            string? baseAddress = configuration["GoogleAPI:UrlAuth"];
            if (!string.IsNullOrEmpty(baseAddress))
            {
                httpClient.BaseAddress = new Uri(baseAddress);
            }
        });

        #endregion

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        //builder.Services.AddApplicationLayer(builder.Configuration);

        builder.Services.AddSingleton<IValidator<CreateRoleCommand>, CreateRolCommandValidation>();
        builder.Services.AddSingleton<IValidator<LoginUserCommand>, LoginCommandValidation>();

        //builder.Services.AddMediatR(c =>
        //c.RegisterServicesFromAssemblyContaining<Program>()
        //.AddBehavior<IPipelineBehavior<CreateRoleCommand, Result<Rol>>>()
        //.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
        //);


        builder.Services.AddSingleton<IRoleRepository, RolRepository>();

        builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        builder.Services.AddMediatR(C =>

        C.RegisterServicesFromAssemblyContaining<Program>()
        .AddBehavior(typeof(IValidationService<>), typeof(ValidationService<>))
        );
        builder.Services.AddValidatorsFromAssemblyContaining<Program>(includeInternalTypes: true);
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}