using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class PasswordHasher
    {
        static string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=nadezhdaa";
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Конкатенация пароля и соли
                byte[] combinedBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);

                // Конвертируем хеш в строку Base64
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); // В строку в формате, похожем на сохраненный хеш
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            string enteredHash = HashPassword(enteredPassword, storedSalt);

            // Отладочное сообщение для проверки
           

            // Сравниваем хранимый хеш с хешем введенного пароля
            return storedHash == enteredHash;
        }




    }
}
