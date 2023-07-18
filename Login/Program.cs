using Login.Context;
using Login.Interface.Repository;
using Login.Interface.Service;
using Login.Ioc;
using Login.Model;
using Login.Repository;
using Login.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<MySqlContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add IHttpContextAccessor to the Pontoiner
builder.Services.AddHttpContextAccessor();

// Register IoC for services
builder.Services.ConfigRepositoryIoc();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MySqlContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Login v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(p => {
    p.AllowAnyMethod();
    p.AllowAnyHeader();
    p.AllowAnyOrigin();
});
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
});

app.Run();

