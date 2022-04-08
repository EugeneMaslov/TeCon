using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TeCon.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Test> Tests { get; set; }
        public override bool Equals(object obj)
        {
            User user = obj as User;
            if (user.Login != null && user.Password != null && Login != null && Password != null)
            {
                return this.Login == user.Login && this.GetHash(Password) == user.Password;
            }
            else return false;
        }
        public string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }
    }
}
