using System.Security.Cryptography;
using AsadTutorialWebAPI.Interfaces;
using AsadTutorialWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AsadTutorialWebAPI.Data.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext dc;

        public UserRepository(DataContext dc) {
            this.dc = dc;
        }

        public async Task<User> Authenticate(string username, string inputPassword)
        {
            var user = await dc.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null || user.PasswordKey == null)
                return null;

            if (!MatchPasswordHash(inputPassword, user.Password, user.PasswordKey))
                return null;

            return user;
        }

        public bool MatchPasswordHash(string inputPassword, byte[] password, byte[] passwordkey)
        {
            byte[] passwordHash;

            using (var a = new HMACSHA512(passwordkey))
            {
                passwordHash = a.ComputeHash(System.Text.Encoding.UTF8.GetBytes(inputPassword));
            }

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != password[i])
                    return false;
            }

            return true;
        }

        public void Register(string username, string password)
        {
            byte[] PasswordHash, PasswordKey;

            using (var h = new HMACSHA512())
            {
                PasswordKey = h.Key;
                PasswordHash = h.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            User user = new User();
            user.Username = username;
            user.Password = PasswordHash;
            user.PasswordKey = PasswordKey;
            dc.Users.Add(user);
        }

        public async Task<bool> UserAlreadyExist(string username)
        {
            return await dc.Users.AnyAsync(x => x.Username == username);
        }
    }
}
