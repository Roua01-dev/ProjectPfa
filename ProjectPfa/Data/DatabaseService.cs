using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ProjectPfa.Models;
namespace ProjectPfa.Data
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection _database;

        // Chemin de la base de données
        private static readonly string DatabasePath = Path.Combine(FileSystem.AppDataDirectory, "properties.db");

        // Initialisation de la base de données
        public static async Task InitializeDatabaseAsync()
        {
            if (_database == null)
            {
                _database = new SQLiteAsyncConnection(DatabasePath);

                // Créez la table Property si elle n'existe pas
                await _database.CreateTableAsync<Property>();
                await _database.CreateTableAsync<User>();

            }
        }

        // Méthode pour insérer une nouvelle propriété
        public static async Task<int> AddPropertyAsync(Property property)
        {
            await InitializeDatabaseAsync();
            return await _database.InsertAsync(property);
        }

        // Méthode pour récupérer toutes les propriétés
        public static async Task<List<Property>> GetPropertiesAsync()
        {
            await InitializeDatabaseAsync();
            return await _database.Table<Property>().ToListAsync();
        }


        public static async Task<List<Property>> GetAllPropertiesAsync()
        {
            await InitializeDatabaseAsync();
            return await _database.Table<Property>().ToListAsync();
        }




        // Add a new user with email uniqueness check
        public static async Task<int> AddUserAsync(User user)
        {
            try
            {
                await InitializeDatabaseAsync();

                // Check if the email already exists
                var existingUser = await _database.Table<User>().FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    throw new Exception("A user with this email already exists.");
                }

                // Hash the password with salt before storing
                user.Password = HashPasswordWithSalt(user.Password, out string salt);
                user.ConfirmPassword = null; // Optional: Do not store confirm password
                user.ProfilePicturePath = user.ProfilePicturePath ?? string.Empty; // Default value

                return await _database.InsertAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                return -1; // Return -1 to indicate failure
            }
        }
        // Get all users
        public static async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                await InitializeDatabaseAsync();
                return await _database.Table<User>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                return new List<User>();
            }
        }

        // Get a specific user by ID
        public static async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                await InitializeDatabaseAsync();
                return await _database.FindAsync<User>(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user: {ex.Message}");
                return null;
            }
        }

        // Update a user
        public static async Task<int> UpdateUserAsync(User user)
        {
            try
            {
                await InitializeDatabaseAsync();
                return await _database.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return -1;
            }
        }

        // Delete a user
        public static async Task<int> DeleteUserAsync(int id)
        {
            try
            {
                await InitializeDatabaseAsync();
                var user = await GetUserByIdAsync(id);
                if (user != null)
                {
                    return await _database.DeleteAsync(user);
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return -1;
            }
        }


        // Verify user credentials
        public static async Task<User> VerifyUserAsync(string email, string password)
        {
            try
            {
                await InitializeDatabaseAsync();

                // Récupérez l'utilisateur
                var user = await _database.Table<User>().FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return null;
                }

                // Hachez le mot de passe saisi avec le sel stocké
                string hashedInputPassword = HashPasswordWithSalt(password, user.Password.Split(':')[1]);
                return hashedInputPassword == user.Password ? user : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying user: {ex.Message}");
                return null;
            }
        }


        // Hash password with salt
        private static string HashPasswordWithSalt(string password, out string salt)
        {
            salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            using (var sha256 = SHA256.Create())
            {
                var combinedPasswordSalt = Encoding.UTF8.GetBytes(password + salt);
                var hash = sha256.ComputeHash(combinedPasswordSalt);
                return $"{Convert.ToBase64String(hash)}:{salt}";
            }
        }

        // Overload for hashing with an existing salt
        private static string HashPasswordWithSalt(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedPasswordSalt = Encoding.UTF8.GetBytes(password + salt);
                var hash = sha256.ComputeHash(combinedPasswordSalt);
                return $"{Convert.ToBase64String(hash)}:{salt}";
            }
        }

        // Get a specific user by email
        public static async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                await InitializeDatabaseAsync();
                return await _database.Table<User>().FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by email: {ex.Message}");
                return null;
            }
        }



        public static class UserSession
        {
            public static int? CurrentUserId { get; private set; }

            public static void LoginUser(int userId)
            {
                CurrentUserId = userId;
            }

            public static void LogoutUser()
            {
                CurrentUserId = null;
            }
        }


    }
}
