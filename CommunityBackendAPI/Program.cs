using CommunityBackendAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 移除身份验证配置
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
 
 // 读取 JWT 配置
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

// 添加 JWT 认证
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // 验证 Issuer
            ValidateAudience = true, // 验证 Audience
            ValidateLifetime = true, // 验证 Token 是否过期
            ValidateIssuerSigningKey = true, // 验证签名
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });
 //


builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 40))));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("http://0.0.0.0:8080");
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});


app.UseDeveloperExceptionPage();  // 仅在开发环境中启用详细错误信息
app.UseHttpsRedirection();  // 如果要启用 HTTPS，请保留此行

app.UseAuthentication(); // 启用身份认证
app.UseAuthorization();  // 如果你希望在全局范围内启用授权，可以保留，但可以暂时移除这行来测试匿名访问

app.MapControllers();
app.Run();