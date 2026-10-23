CREATE DATABASE IF NOT EXISTS titanic_db
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE titanic_db;

CREATE TABLE IF NOT EXISTS heart_disease_predictions (
    id BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
    age SMALLINT UNSIGNED NOT NULL,
    sex CHAR(1) NOT NULL,
    chest_pain_type VARCHAR(3) NOT NULL,
    resting_bp SMALLINT UNSIGNED NOT NULL,
    cholesterol SMALLINT UNSIGNED NOT NULL,
    fasting_bs TINYINT UNSIGNED NOT NULL,
    resting_ecg VARCHAR(6) NOT NULL,
    max_hr SMALLINT UNSIGNED NOT NULL,
    exercise_angina CHAR(1) NOT NULL,
    st_slope VARCHAR(5) NOT NULL,
    predicted_heart_disease BOOLEAN NOT NULL,
    model_score DECIMAL(9, 8) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id)
);
