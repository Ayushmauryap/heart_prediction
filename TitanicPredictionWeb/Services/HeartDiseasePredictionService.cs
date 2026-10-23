using System.Reflection;
using System.Text.Json;
using HeartHealthWeb.Models;

namespace HeartHealthWeb.Services;

public sealed class HeartDiseasePredictionService
{
    private readonly HeartDiseaseModelData model;

    public HeartDiseasePredictionService()
    {
        const string resourceName = "HeartHealthWeb.Models.heart-disease-model.json";
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
            ?? throw new InvalidDataException($"The embedded heart model resource '{resourceName}' is missing.");

        model = JsonSerializer.Deserialize<HeartDiseaseModelData>(stream, new JsonSerializerOptions(JsonSerializerDefaults.Web))
            ?? throw new InvalidDataException("The embedded heart model could not be read.");

        ValidateModel();
    }

    public HeartDiseasePredictionResult Predict(HeartDiseaseInput input)
    {
        var transformed = new[]
        {
            Encode(input.ChestPainType, model.EncodedCategories.ChestPainType, nameof(input.ChestPainType)),
            Encode(input.RestingECG, model.EncodedCategories.RestingECG, nameof(input.RestingECG)),
            Encode(input.ExerciseAngina, model.EncodedCategories.ExerciseAngina, nameof(input.ExerciseAngina)),
            Encode(input.STSlope, model.EncodedCategories.ST_Slope, nameof(input.STSlope)),
            Encode(input.Sex, model.EncodedCategories.Sex, nameof(input.Sex)),
            (double)input.Age,
            input.RestingBP,
            input.Cholesterol,
            input.FastingBS,
            input.MaxHR
        };

        var scaled = new List<double>(transformed.Length);
        for (var i = 0; i < transformed.Length; i++)
        {
            if (model.SelectedFeatures[i])
            {
                scaled.Add(transformed[i] * model.ScalerScales[i] + model.ScalerOffsets[i]);
            }
        }

        var node = 0;
        while (model.SplitFeatures[node] >= 0)
        {
            var feature = model.SplitFeatures[node];
            node = scaled[feature] <= model.SplitThresholds[node]
                ? model.ChildrenLeft[node]
                : model.ChildrenRight[node];
        }

        var score = model.PositiveProbabilities[node];
        return new HeartDiseasePredictionResult(score > 0.5, score);
    }

    private double Encode(string value, string[] categories, string featureName)
    {
        var index = Array.IndexOf(categories, value);
        return index >= 0
            ? index
            : throw new InvalidOperationException($"The heart model does not support the supplied {featureName} value.");
    }

    private void ValidateModel()
    {
        var nodeCount = model.SplitFeatures.Length;
        if (model.ChildrenLeft.Length != nodeCount
            || model.ChildrenRight.Length != nodeCount
            || model.SplitThresholds.Length != nodeCount
            || model.PositiveProbabilities.Length != nodeCount
            || model.SelectedFeatures.Length != model.ScalerScales.Length
            || model.SelectedFeatures.Length != model.ScalerOffsets.Length
            || model.SelectedFeatures.Length != 10)
        {
            throw new InvalidDataException("The embedded heart model contains inconsistent feature or tree data.");
        }
    }

    private sealed class HeartDiseaseModelData
    {
        public HeartDiseaseModelData()
        {
        }

        public EncodedCategoryData EncodedCategories { get; init; } = new();
        public double[] ScalerScales { get; init; } = [];
        public double[] ScalerOffsets { get; init; } = [];
        public bool[] SelectedFeatures { get; init; } = [];
        public int[] ChildrenLeft { get; init; } = [];
        public int[] ChildrenRight { get; init; } = [];
        public int[] SplitFeatures { get; init; } = [];
        public double[] SplitThresholds { get; init; } = [];
        public double[] PositiveProbabilities { get; init; } = [];
    }

    private sealed class EncodedCategoryData
    {
        public EncodedCategoryData()
        {
        }

        public string[] ChestPainType { get; init; } = [];
        public string[] RestingECG { get; init; } = [];
        public string[] ExerciseAngina { get; init; } = [];
        public string[] ST_Slope { get; init; } = [];
        public string[] Sex { get; init; } = [];
    }
}
