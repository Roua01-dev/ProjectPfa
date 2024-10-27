using System.Collections.Generic;

namespace ProjectPfa.Models
{
    public static class PropertyRepo
    {
        public static List<Property> AllProperties => new List<Property>
        {
            new Property
            {
                Title = "Charming House in Medina",
                Address = "Médina, Tunis",
                Price = 472,
                DefaultImage = "medina1.jpg",
                Images = new List<string>
                {
                    "medina1.jpg",
                    "medina2.jpg",
                    "medina3.jpg"
                },
                SquareFootage = 1416,
                Bedrooms = 4,
                Bathrooms = 2,
                Garage = 2,
                Description = "A charming house located in the heart of Medina, featuring spacious rooms and a beautiful garden.",
                PhoneNumber = "123-456-7890",
                IsTrending = true,
                IsPopular = false,
                IsForSale = true,
                IsForRent = false
            },
            new Property
            {
                Title = "Patio in Medina",
                Address = "Médina, Tunis",
                Price = 472,
                DefaultImage = "patio.jpg",
                Images = new List<string>
                {
                    "patio.jpg",
                    "patio1.jpg",
                    "patio2.jpg",
                    "patio3.jpg"
                },
                SquareFootage = 1500,
                Bedrooms = 3,
                Bathrooms = 2,
                Garage = 1,
                Description = "A lovely patio situated in the historic Medina area, ideal for outdoor gatherings.",
                PhoneNumber = "234-567-8901",
                IsTrending = false,
                IsPopular = true,
                IsForSale = false,
                IsForRent = true
            },
            new Property
            {
                Title = "Modern Apartment in Carthage",
                Address = "Jardins de Carthage, Tunis",
                Price = 472,
                DefaultImage = "carthage.jpg",
                Images = new List<string>
                {
                    "carthage.jpg",
                    "carthage1.jpg",
                    "carthage2.jpg",
                    "carthage3.jpg"
                },
                SquareFootage = 1300,
                Bedrooms = 2,
                Bathrooms = 2,
                Garage = 1,
                Description = "A modern apartment with all the latest amenities, located in the prestigious Jardins de Carthage.",
                PhoneNumber = "345-678-9012",
                IsTrending = true,
                IsPopular = true,
                IsForSale = true,
                IsForRent = false
            },
            new Property
            {
                Title = "Lakeside Residence",
                Address = "Berges du lac 1, Tunis",
                Price = 472,
                DefaultImage = "lac1.jpg",
                Images = new List<string>
                {
                    "lac1.jpg",
                    "lac11.jpg",
                    "lac12.jpg",
                    "lac13.jpg",
                    "lac14.jpg"
                },
                SquareFootage = 1600,
                Bedrooms = 4,
                Bathrooms = 3,
                Garage = 2,
                Description = "A serene lakeside residence offering breathtaking views and ample space.",
                PhoneNumber = "456-789-0123",
                IsTrending = false,
                IsPopular = false,
                IsForSale = false,
                IsForRent = true
            }
        };
    }
}
