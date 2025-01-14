using CommunityBackendAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// �Ƴ������֤����
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

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


app.UseDeveloperExceptionPage();  // ���ڿ���������������ϸ������Ϣ
app.UseHttpsRedirection();  // ���Ҫ���� HTTPS���뱣������

app.UseAuthorization();  // �����ϣ����ȫ�ַ�Χ��������Ȩ�����Ա�������������ʱ�Ƴ�������������������

app.MapControllers();
app.Run();