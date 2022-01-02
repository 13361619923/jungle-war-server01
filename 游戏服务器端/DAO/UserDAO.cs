using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GameServer.Model;
namespace GameServer.DAO
{
    /// <summary>
    /// 用户直接操作数据库
    /// </summary>
    class UserDAO
    {
        /// <summary>
        /// 校验用户名和密码
        /// </summary>
        /// <param name="conn">数据库链接</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public User VerifyUser(MySqlConnection conn, string username, string password)
        {
            MySqlDataReader reader = null;//用于接收执行查询语句的返回值
            try
            {
                //创建一个SQL命令
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username and password = @password", conn);//查询用户的SQL语句
                cmd.Parameters.AddWithValue("username", username);//设置username参数
                cmd.Parameters.AddWithValue("password", password);//设置password参数
                reader = cmd.ExecuteReader();//执行查询语句
                if (reader.Read())//如果返回true则查询到了数据
                {
                    int id = reader.GetInt32("id");     //读取id列数据
                    User user = new User(id, username, password);
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在VerifyUser的时候出现异常：" + e);//报异常
            }
            finally
            {
                if (reader != null) reader.Close();//关闭数据库链接资源
            }
            return null;
        }

        public bool GetUserByUsername(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username", conn);
                cmd.Parameters.AddWithValue("username", username);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetUserByUsername的时候出现异常：" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return false;
        }

        public void AddUser(MySqlConnection conn, string username, string password)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into user set username = @username , password = @password", conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("在AddUser的时候出现异常：" + e);
            }
        }
    }
}
