using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    /// <summary>
    /// 与数据库中的信息对应，用于登录和注册
    /// </summary>
    class User
    {
        public User(int id,string username,string password)
        {
            this.Id = id;
            this.Username = username;
            this.Password = password;
        }
        public int Id { get; set; }     //ID
        public string Username { get; set; }//用户名   
        public string Password { get; set; }//密码
    }
}
