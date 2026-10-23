# Heartwise — Heart Health Screening

An ASP.NET Core Razor Pages educational website that reproduces the supplied heart-disease model and saves screening inputs and model results to MySQL.

## Requirements

- .NET 8 SDK
- MySQL Server

## MySQL setup

Run [`database/init.sql`](database/init.sql) against MySQL. It keeps the existing `titanic_db` database and existing tables intact, and creates a separate `heart_disease_predictions` table for this website.

The start script prompts for the local MySQL `root` password without saving it in the project. From the workspace folder, run:

```powershell
.\TitanicPredictionWeb\Start-HeartHealth.ps1
```

The site is available at `http://127.0.0.1:5188`. To use another database or server, pass `-Database`, `-Server`, or `-Port` to the script. The default database remains `titanic_db`.

## Model behavior

The supplied `pipe.pkl` was exported as embedded static model data, including its categorical encoders, scaler, feature selection, and fitted model. The website uses the model's 10 expected inputs: Age, Sex, ChestPainType, RestingBP, Cholesterol, FastingBS, RestingECG, MaxHR, ExerciseAngina, and ST_Slope. `Oldpeak` is not accepted by this fitted pipeline.

The displayed score is the selected model leaf's class share. It is not a calibrated personal probability, diagnosis, or substitute for professional medical care. The app does not need Python or scikit-learn at runtime.

## Data preservation

The SQL setup does not drop or alter the existing database or Titanic `predictions` table. Heart screening records are written to the new `heart_disease_predictions` table.
