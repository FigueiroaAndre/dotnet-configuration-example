using System.ComponentModel.DataAnnotations;

namespace ConfigurationExample.Configuration;

public class GeneralConfiguration
{
    public const string Section = "GeneralSettings";

    [Required]
    public string Settings1 { get; set; } = string.Empty;

    [Required]
    public string Settings2 { get; set; } = string.Empty;

    [Required]
    public string Settings3 { get; set; } = string.Empty;

    [Required]
    public string Settings4 { get; set; } = string.Empty;

    [Required]
    public string Settings5 { get; set; } = string.Empty;

    [Required]
    public string Settings6 { get; set; } = string.Empty;

    [Required]
    public string Settings7 { get; set; } = string.Empty;
}

