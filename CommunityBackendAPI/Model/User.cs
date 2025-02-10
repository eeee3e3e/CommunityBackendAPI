namespace CommunityBackendAPI.Model
{
    public class User
    {
        public int Id { get; set; } // 用户 ID
        public string OpenId { get; set; } // 微信 OpenID
        public string Token { get; set; } // 存储 JWT 令牌
    }
}
