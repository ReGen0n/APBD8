namespace APBD8.DTOs;

using System.ComponentModel.DataAnnotations;

public class PCRequestDto
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;

    [Required]
    public double Weight { get; set; }

    [Required]
    public int Warranty { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public int Stock { get; set; }
}