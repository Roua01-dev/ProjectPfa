using Newtonsoft.Json;
using SQLite;
using System.Collections.Generic;

namespace ProjectPfa.Models
{
    public class Property
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }

        public string DefaultImage { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }

        // Utilisation de ImagesJson pour stocker la liste d'images
        public string ImagesJson { get; set; }

        // Propriété d'images qui utilise la sérialisation/désérialisation JSON
        [Ignore] // Ignore cette propriété lors de l'enregistrement dans la base de données
        public List<string> Images
        {
            get => string.IsNullOrEmpty(ImagesJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(ImagesJson);
            set => ImagesJson = JsonConvert.SerializeObject(value);
        }

        public double SquareFootage { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Garage { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsTrending { get; set; }
        public bool IsPopular { get; set; }
        public bool IsForSale { get; set; }
        public bool IsForRent { get; set; }
        public bool IsAll { get; set; } = true; // Par défaut à vrai pour la catégorie "All"
    }
}
