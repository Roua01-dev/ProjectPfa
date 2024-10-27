using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using ProjectPfa.Models;

namespace ProjectPfa.Data
{
    public class PropertyDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public PropertyDatabase()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ProjectPfa.db3");
            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<Property>().Wait(); // Cette ligne doit créer la table Property dans la base de données
        }

        public Task<int> SavePropertyAsync(Property property)
        {
            return _database.InsertAsync(property);
        }

        public Task<List<Property>> GetPropertiesAsync()
        {
            return _database.Table<Property>().ToListAsync();
        }

        public Task<Property> GetPropertyAsync(int id)
        {
            return _database.Table<Property>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<int> UpdatePropertyAsync(Property property)
        {
            return _database.UpdateAsync(property);
        }

        public Task<int> DeletePropertyAsync(Property property)
        {
            return _database.DeleteAsync(property);
        }

    }
}
