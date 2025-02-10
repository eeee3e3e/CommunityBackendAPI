using CommunityBackendAPI.Model;
using System.Collections.Generic;
using System.Linq;

public class UserService
{
    private static List<User> _users = new(); // 模拟数据库（这里用内存存储）

    public User GetUserByOpenId(string openId)
    {
        return _users.FirstOrDefault(u => u.OpenId == openId);
    }

    public void AddUser(User user)
    {
        _users.Add(user);
    }
}