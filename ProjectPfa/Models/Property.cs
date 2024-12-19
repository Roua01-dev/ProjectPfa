using SQLite;

public class Property
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } // Clé primaire auto-générée

    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Address { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public int Garage { get; set; }
    public string PropertyStatus { get; set; }
    public string DefaultImage { get; set; }
    public string Images { get; set; } // Images séparées par des virgules
    public string Description { get; set; }
    public string Numero { get; set; } // Champ pour le numéro de téléphone

    // Champs pour les statuts
    public bool IsTrending { get; set; }
    public bool IsPopular { get; set; }
    public bool IsForRent { get; set; }
    public bool IsForSale { get; set; }
    public bool IsAll { get; set; } // Indique si la propriété appartient à "All"
}
