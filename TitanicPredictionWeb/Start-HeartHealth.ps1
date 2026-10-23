param(
    [string]$Server = "localhost",
    [uint16]$Port = 3306,
    [string]$Database = "titanic_db"
)

$projectPath = Join-Path $PSScriptRoot "TitanicPredictionWeb.csproj"
if (-not (Test-Path $projectPath)) {
    throw "Could not find the web project at '$projectPath'."
}

$mysqlUsername = "root"
$securePassword = Read-Host "Enter the MySQL password for account '$mysqlUsername' on $Server" -AsSecureString
$passwordPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
$builder = [System.Data.Common.DbConnectionStringBuilder]::new()
$builder["Server"] = $Server
$builder["Port"] = $Port
$builder["Database"] = $Database
$builder["User ID"] = $mysqlUsername
$builder["Password"] = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($passwordPointer)

$env:ConnectionStrings__HeartDiseaseDatabase = $builder.ConnectionString
try {
    Push-Location $PSScriptRoot
    try {
        & dotnet run --project $projectPath --urls "http://127.0.0.1:5188"
        if ($LASTEXITCODE -ne 0) {
            throw "The Heartwise website exited with code $LASTEXITCODE."
        }
    }
    finally {
        Pop-Location
    }
}
finally {
    Remove-Item Env:ConnectionStrings__HeartDiseaseDatabase -ErrorAction SilentlyContinue
    $builder["Password"] = ""
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($passwordPointer)
    $securePassword.Dispose()
}
