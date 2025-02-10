using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using CommunityBackendAPI.Model;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly UserService _userService;
    private readonly HttpClient _httpClient;

    public AuthController(IConfiguration config, UserService userService, HttpClient httpClient)
    {
        _config = config;
        _userService = userService;
        _httpClient = httpClient;
    }

    [HttpPost("wechat-login")]
    public async Task<IActionResult> WeChatLogin([FromBody] WeChatLoginRequest request)
    {
        // 微信小程序 appid 和 secret
        string appId = "wxd711e382317b5c97";
        string appSecret = "68f0da3abc2d5d32b8695c761f94c726";

        // 微信 API 获取 openid 和 session_key
        string url = $"https://api.weixin.qq.com/sns/jscode2session?appid={appId}&secret={appSecret}&js_code={request.Code}&grant_type=authorization_code";
        var response = await _httpClient.GetStringAsync(url);
        var json = JObject.Parse(response);

        if (json["openid"] == null)
        {
            return BadRequest(new { message = "微信登录失败" });
        }

        string openId = json["openid"].ToString();

        // 检查用户是否已存在
        var user = _userService.GetUserByOpenId(openId);
        if (user == null)
        {
            user = new User { OpenId = openId };
            _userService.AddUser(user);
        }

        // 生成 JWT
        string token = GenerateJwtToken(openId);

        // 更新用户 Token
        user.Token = token;

        return Ok(new { token });
    }

    private string GenerateJwtToken(string openId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, openId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2), // Token 2 小时有效
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

// 微信登录请求数据模型
public class WeChatLoginRequest
{
    public string Code { get; set; }
}