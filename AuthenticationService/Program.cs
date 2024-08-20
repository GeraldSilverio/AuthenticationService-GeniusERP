using AuthenticationService.Api.Entities;
using AuthenticationService.Api.Features.Roles.Request;
using AuthenticationService.Api.Features.Roles.Validations;
using AuthenticationService.Api.Results;
using AuthenticationService.Application;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Services;
using AuthenticationService.Application.Validations;
using AuthenticationService.Security;
using FluentValidation;
using MediatR;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddSecurityLayer();


builder.Services.AddSingleton<IValidator<CreateRoleCommand>, CreateRolCommandValidation>();

//builder.Services.AddMediatR(c =>
//c.RegisterServicesFromAssemblyContaining<Program>()
//.AddBehavior<IPipelineBehavior<CreateRoleCommand, Result<Rol>>>()
//.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
//);


builder.Services.AddSingleton<IRoleRepository, RolRepository>();

builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddMediatR(C =>

C.RegisterServicesFromAssemblyContaining<Program>()

);
builder.Services.AddValidatorsFromAssemblyContaining<Program>(includeInternalTypes:true);
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
