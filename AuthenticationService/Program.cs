using AuthenticationService.Api.Roles.Interface;
using AuthenticationService.Api.Roles.Repository;
using AuthenticationService.Api.Roles.Request;
using AuthenticationService.Api.Roles.Validations;
using AuthenticationService.Api.User.Repositories;
using AuthenticationService.Api.UserRole.Repository;
using AuthenticationService.Application.Services;
using FirebaseAdmin;
using FluentValidation;
using Google.Apis.Auth.OAuth2;
using MediatR;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IValidator<CreateRoleCommand>, CreateRolCommandValidation>();
builder.Services.AddSingleton<IRoleRepository, RolRepository>();
builder.Services.AddSingleton<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddMediatR(C =>

C.RegisterServicesFromAssemblyContaining<Program>()

);

builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile("FireBase.json")
}));


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
