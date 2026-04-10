# Kafka Test Adımları

## 1. Kafka UI'ye Giriş
- URL: http://localhost:8084
- Cluster: local
- Topics sekmesine git

## 2. Consumer Group Kontrolü
- Consumer Groups sekmesinde `catalog-service-group` görmeli siniz
- Eğer yoksa, Catalog API yeniden başlatın

## 3. Mesaj Gönderme Testi

### Option A: Catalog API üzerinden test
```bash
# PowerShell'de
Invoke-RestMethod -Uri "http://localhost:5100/api/v1.0/course" `
  -Method POST `
  -Headers @{"Content-Type"="multipart/form-data"} `
  -Form @{
    name="Test Course"
    description="Test Description for Kafka"
    price=99.99
    categoryId="<valid-category-guid>"
  }
```

### Option B: Kafka UI'den manuel mesaj gönder
1. Kafka UI'de `course-picture-uploaded` topic'ine git
2. "Produce Message" butonuna tıkla
3. Test mesajı gönder

## 4. Consumer Log Kontrolü
Catalog API loglarında şunu görmeli siniz:
```
Configured endpoint catalog-service-group, Consumer: CoursePictureUploadedEventConsumer
```

## 5. Sorun Giderme

### Port 9094 Erişilebilir mi?
```powershell
Test-NetConnection -ComputerName localhost -Port 9094
# TcpTestSucceeded: True olmalı
```

### Kafka Container Çalışıyor mu?
```bash
docker ps --filter "name=kafka"
```

### Catalog API Logları
```bash
# Çalışan Catalog API loglarını takip et
# Visual Studio Output penceresinden veya konsol loglarından
```

## Olası Sorunlar

### 1. Topic Auto-Create Çalışmıyor
- Docker-compose.yml'de `KAFKA_AUTO_CREATE_TOPICS_ENABLE: true` var
- Eğer topic'ler oluşmuyorsa, manuel oluşturun:
  - Kafka UI → Topics → Create Topic
  - Name: `course-picture-uploaded`
  - Partitions: 3

### 2. Consumer Bağlanamıyor
- `BusOptions` yapılandırmasını kontrol et
- `localhost:9094` doğru mu?
- Firewall sorunu olabilir

### 3. Mesajlar Consume Edilmiyor
- Consumer group offset'lerini kontrol et
- `AutoOffsetReset = Earliest` ayarlandı, eski mesajları da okumalı
