using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectPfa.Models;
using ProjectPfa.Data;

namespace ProjectPfa.Services
{
    public class PropertyService
    {
        private readonly PropertyDatabase _database;

        public PropertyService()
        {
            _database = new PropertyDatabase(); // S'assurer que le constructeur fonctionne sans erreur
        }

        public async Task SaveAllPropertiesAsync()
        {
            var properties = PropertyRepo.AllProperties; // Assurez-vous que cette propriété existe
            int savedCount = 0;

            foreach (var property in properties)
            {
                await _database.SavePropertyAsync(property);
                savedCount++;
                Console.WriteLine($"Property '{property.Title}' saved successfully."); // Afficher le message de succès
            }

            Console.WriteLine($"Total properties saved: {savedCount}"); // Afficher le total enregistré
        }
    }
}
