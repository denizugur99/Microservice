# Test API Script
$baseUrl = "http://localhost:5100"

Write-Host "`n=== GET Categories ===" -ForegroundColor Cyan
$getResponse = Invoke-RestMethod -Uri "$baseUrl/app/categories" -Method GET
$getResponse | ConvertTo-Json -Depth 10

Write-Host "`n=== POST New Category ===" -ForegroundColor Cyan
$body = @{
    categoryName = "PowerShell Test"
} | ConvertTo-Json

$postResponse = Invoke-RestMethod -Uri "$baseUrl/app/categories" -Method POST -Body $body -ContentType "application/json"
$postResponse | ConvertTo-Json -Depth 10

Write-Host "`n=== Test Completed ===" -ForegroundColor Green
