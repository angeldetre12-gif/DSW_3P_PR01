using System.ComponentModel.DataAnnotations;

namespace DSW_3P_EJ01.Models;

public class Mision
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(Fácil|Media|Difícil)$", ErrorMessage = "La dificultad debe ser Fácil, Media o Difícil.")]
    public string Dificultad { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "La recompensa XP no puede ser negativa.")]
    public int RecompensaXP { get; set; }

    public bool Completada { get; set; }
}
