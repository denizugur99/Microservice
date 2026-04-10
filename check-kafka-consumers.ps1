# Kafka Consumer Durumu Kontrol Scripti

Write-Host "`n=== KAFKA CONSUMER DURUMU ===" -ForegroundColor Cyan

# 1. Catalog API çalışıyor mu?
$catalogProcess = Get-Process -Name "Microservice.Catalog.Api" -ErrorAction SilentlyContinue
if ($catalogProcess) {
    Write-Host "[OK] Catalog API çalışıyor (PID: $($catalogProcess.Id))" -ForegroundColor Green
} else {
    Write-Host "[HATA] Catalog API çalışmıyor!" -ForegroundColor Red
    Write-Host "Lütfen Catalog API'yi başlatın" -ForegroundColor Yellow
}

# 2. Kafka Container durumu
Write-Host "`n=== KAFKA CONTAINER ===" -ForegroundColor Cyan
docker ps --filter "name=kafka-1" --format "table {{.Names}}\t{{.Status}}"

# 3. Kafka UI erişimi
Write-Host "`n=== KAFKA UI ===" -ForegroundColor Cyan
try {
    $response = Invoke-WebRequest -Uri "http://localhost:8084" -UseBasicParsing -TimeoutSec 3
    Write-Host "[OK] Kafka UI erişilebilir: http://localhost:8084" -ForegroundColor Green
} catch {
    Write-Host "[HATA] Kafka UI erişilemiyor!" -ForegroundColor Red
}

# 4. Topics
Write-Host "`n=== TOPICS ===" -ForegroundColor Cyan
Write-Host "Tarayıcıda kontrol edin:"
Write-Host "http://localhost:8084/ui/clusters/local/topics" -ForegroundColor Yellow

# 5. Consumer Groups
Write-Host "`n=== CONSUMER GROUPS ===" -ForegroundColor Cyan
Write-Host "Tarayıcıda kontrol edin:"
Write-Host "http://localhost:8084/ui/clusters/local/consumer-groups" -ForegroundColor Yellow
Write-Host "`nAranacak grup: catalog-service-group" -ForegroundColor White

Write-Host "`n=== KONTROL ADIMLARI ===" -ForegroundColor Cyan
Write-Host "1. Consumer Groups sekmesine git"
Write-Host "2. 'catalog-service-group' var mı kontrol et"
Write-Host "3. Gruba tıkla ve 'Lag' değerine bak:"
Write-Host "   - Lag = 0  -> Tüm mesajlar consume edildi ✓" -ForegroundColor Green
Write-Host "   - Lag > 0  -> Mesajlar bekliyor ✗" -ForegroundColor Yellow
Write-Host "`n"
