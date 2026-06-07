using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Key]
    public int id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Category { get; set; }

    public decimal CurrentPrice { get; set; }

    public decimal? OldPrice { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    // Stored as JSON or separate table (see note below)
    public List<string> AvailableSizes { get; set; }

    public List<string> Tags { get; set; }

    public int Stock { get; set; }

    public string Brand { get; set; }

    public double Rating { get; set; }

    public bool IsNew { get; set; }
}