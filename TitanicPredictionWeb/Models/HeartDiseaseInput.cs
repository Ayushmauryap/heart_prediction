using System.ComponentModel.DataAnnotations;

namespace HeartHealthWeb.Models;

public sealed class HeartDiseaseInput
{
    [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
    public int Age { get; set; } = 45;

    [Required]
    [RegularExpression("^(M|F)$", ErrorMessage = "Choose a valid sex.")]
    public string Sex { get; set; } = "M";

    [Required]
    [RegularExpression("^(ASY|NAP|ATA|TA)$", ErrorMessage = "Choose a valid chest pain type.")]
    [Display(Name = "Chest pain type")]
    public string ChestPainType { get; set; } = "ATA";

    [Range(0, 300, ErrorMessage = "Resting blood pressure must be between 0 and 300 mm Hg.")]
    [Display(Name = "Resting blood pressure (mm Hg)")]
    public int RestingBP { get; set; } = 120;

    [Range(0, 1000, ErrorMessage = "Cholesterol must be between 0 and 1,000 mg/dL.")]
    [Display(Name = "Cholesterol (mg/dL)")]
    public int Cholesterol { get; set; } = 200;

    [Range(0, 1, ErrorMessage = "Choose 0 or 1.")]
    [Display(Name = "Fasting blood sugar above 120 mg/dL")]
    public int FastingBS { get; set; }

    [Required]
    [RegularExpression("^(Normal|ST|LVH)$", ErrorMessage = "Choose a valid resting ECG result.")]
    [Display(Name = "Resting ECG")]
    public string RestingECG { get; set; } = "Normal";

    [Range(0, 300, ErrorMessage = "Maximum heart rate must be between 0 and 300 bpm.")]
    [Display(Name = "Maximum heart rate (bpm)")]
    public int MaxHR { get; set; } = 150;

    [Required]
    [RegularExpression("^(Y|N)$", ErrorMessage = "Choose yes or no.")]
    [Display(Name = "Exercise-induced angina")]
    public string ExerciseAngina { get; set; } = "N";

    [Required]
    [RegularExpression("^(Flat|Up|Down)$", ErrorMessage = "Choose a valid ST slope.")]
    [Display(Name = "ST slope")]
    public string STSlope { get; set; } = "Up";
}
