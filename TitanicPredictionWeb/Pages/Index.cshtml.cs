using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HeartHealthWeb.Models;
using HeartHealthWeb.Services;
using MySqlConnector;

namespace HeartHealthWeb.Pages;

public sealed class IndexModel(
    HeartDiseasePredictionService predictionService,
    HeartDiseaseRepository predictionRepository,
    ILogger<IndexModel> logger) : PageModel
{
    [BindProperty]
    public HeartDiseaseInput Patient { get; set; } = new();

    public HeartDiseasePredictionResult? Result { get; private set; }
    public bool SavedToDatabase { get; private set; }
    public string? DatabaseMessage { get; private set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Result = predictionService.Predict(Patient);

        try
        {
            await predictionRepository.SaveAsync(Patient, Result);
            SavedToDatabase = true;
        }
        catch (MySqlException exception)
        {
            logger.LogError(exception, "Could not save a heart disease prediction to MySQL.");
            DatabaseMessage = "The model result was calculated, but it could not be saved. Check that MySQL is running, the heart predictions table exists, and the connection string is correct.";
        }
        catch (InvalidOperationException exception)
        {
            logger.LogError(exception, "MySQL connection configuration is missing or invalid.");
            DatabaseMessage = exception.Message;
        }

        return Page();
    }
}
