using HeartHealthWeb.Models;
using MySqlConnector;

namespace HeartHealthWeb.Services;

public sealed class HeartDiseaseRepository(IConfiguration configuration)
{
    public async Task SaveAsync(HeartDiseaseInput input, HeartDiseasePredictionResult result)
    {
        var connectionString = configuration.GetConnectionString("HeartDiseaseDatabase");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Set the ConnectionStrings__HeartDiseaseDatabase environment variable before saving predictions.");
        }

        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        const string sql = """
            INSERT INTO heart_disease_predictions
                (age, sex, chest_pain_type, resting_bp, cholesterol, fasting_bs, resting_ecg,
                 max_hr, exercise_angina, st_slope, predicted_heart_disease, model_score)
            VALUES
                (@age, @sex, @chestPainType, @restingBP, @cholesterol, @fastingBS, @restingECG,
                 @maxHR, @exerciseAngina, @stSlope, @predictedHeartDisease, @modelScore);
            """;

        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@age", input.Age);
        command.Parameters.AddWithValue("@sex", input.Sex);
        command.Parameters.AddWithValue("@chestPainType", input.ChestPainType);
        command.Parameters.AddWithValue("@restingBP", input.RestingBP);
        command.Parameters.AddWithValue("@cholesterol", input.Cholesterol);
        command.Parameters.AddWithValue("@fastingBS", input.FastingBS);
        command.Parameters.AddWithValue("@restingECG", input.RestingECG);
        command.Parameters.AddWithValue("@maxHR", input.MaxHR);
        command.Parameters.AddWithValue("@exerciseAngina", input.ExerciseAngina);
        command.Parameters.AddWithValue("@stSlope", input.STSlope);
        command.Parameters.AddWithValue("@predictedHeartDisease", result.HasHeartDisease);
        command.Parameters.AddWithValue("@modelScore", result.ModelScore);

        await command.ExecuteNonQueryAsync();
    }
}
