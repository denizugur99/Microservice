# Email Service - User Secrets Setup

## Development Ortamı İçin

Email şifresi güvenlik nedeniyle git'te tutulmuyor. Geliştirme ortamında çalıştırmak için:

```bash
cd microservice.Email
dotnet user-secrets set "EmailSettings:Password" "YOUR_GMAIL_APP_PASSWORD_HERE"
```

## Gmail App Password Nasıl Alınır?

1. Google Hesabınıza gidin: https://myaccount.google.com/
2. Security > 2-Step Verification (aktif olmalı)
3. App passwords > Create new app password
4. "Mail" ve "Windows Computer" seçin
5. Oluşan 16 haneli şifreyi kopyalayın
6. Yukarıdaki komutu çalıştırıp şifreyi kaydedin

## Production Ortamı

Production'da environment variable kullanın:
```bash
EmailSettings__Password=your-password-here
```

## Mevcut Email Ayarları

- SMTP Server: smtp.gmail.com
- Port: 587
- Sender: budemy.courses@gmail.com
- SSL: Enabled
