using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationExample.Configuration;

[Index(nameof(Settings.Key), IsUnique = true)]
public class Settings
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Key { get; set; } = string.Empty;

    [StringLength(300)]
    [Required]
    public string Value { get; set; } = string.Empty;
}
