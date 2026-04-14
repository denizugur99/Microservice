using Confluent.Kafka;
using microservice.Email.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace microservice.Email.Services
{
    public class EmailService(EmailSettings emailSettings)
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using var smtpClient = new SmtpClient(emailSettings.SmtpServer, emailSettings.SmtpPort)
                {
                    Credentials = new NetworkCredential(emailSettings.SenderEmail, emailSettings.Password),
                    EnableSsl = emailSettings.EnableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailSettings.SenderEmail, emailSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
               
            }
            catch (Exception ex)
            {
               throw new Exception($"Failed to send email to {toEmail}. Error: {ex.Message}", ex);
                
            }
        }

        public async Task SendOrderCreatedEmailAsync(string email, string orderId)
        {
            var subject = "🎉 Sipariş Onayı - Budemy Platform";

            var body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .order-box {{ background: white; padding: 20px; margin: 20px 0; border-radius: 8px; border-left: 4px solid #667eea; }}
        .button {{ display: inline-block; background: #667eea; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; margin: 15px 0; }}
        .footer {{ text-align: center; margin-top: 30px; font-size: 12px; color: #777; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🎓 Budemy Platform</h1>
            <p>Siparişiniz Başarıyla Oluşturuldu!</p>
        </div>
        <div class='content'>
            <p>Merhaba,</p>
            <p>Siparişiniz başarıyla alındı ve işleme alındı. Kurs kayıtlarınız tamamlandıktan sonra hesabınızda görünecektir.</p>

            <div class='order-box'>
                <h3>📦 Sipariş Detayları</h3>
                <p><strong>Sipariş ID:</strong> {orderId}</p>
                <p><strong>Durum:</strong> İşleniyor</p>
            </div>

            <p>Kurslarınıza erişmek için platformumuza giriş yapabilirsiniz:</p>
            <a href='https://budemy.com/my-courses' class='button'>Kurslarıma Git</a>

            <p>Herhangi bir sorunuz varsa, destek ekibimizle iletişime geçmekten çekinmeyin.</p>

            <p>İyi öğrenmeler dileriz! 🚀</p>
            <p><strong>Budemy Ekibi</strong></p>
        </div>
        <div class='footer'>
            <p>Bu otomatik bir e-postadır, lütfen yanıtlamayınız.</p>
            <p>© 2024 Budemy Platform. Tüm hakları saklıdır.</p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendDiscountNotificationEmailAsync(string email, string discountCode)
        {
            var subject = "🎁 Özel İndirim Kodunuz Hazır!";

            var body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .discount-code {{ background: white; padding: 25px; margin: 25px 0; text-align: center; border-radius: 8px; border: 2px dashed #f5576c; }}
        .code {{ font-size: 32px; font-weight: bold; color: #f5576c; letter-spacing: 3px; margin: 15px 0; }}
        .button {{ display: inline-block; background: #f5576c; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; margin: 15px 0; }}
        .features {{ background: white; padding: 20px; margin: 20px 0; border-radius: 8px; }}
        .feature-item {{ padding: 10px 0; border-bottom: 1px solid #eee; }}
        .footer {{ text-align: center; margin-top: 30px; font-size: 12px; color: #777; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🎓 Budemy Platform</h1>
            <h2>Sizin İçin Özel İndirim!</h2>
        </div>
        <div class='content'>
            <p>Merhaba,</p>
            <p>Harika haberlerimiz var! Sizin için özel bir indirim kodu hazırladık. 🎉</p>

            <div class='discount-code'>
                <p style='margin: 0; font-size: 14px; color: #666;'>İndirim Kodunuz:</p>
                <div class='code'>{discountCode}</div>
                <p style='margin: 0; font-size: 14px; color: #666;'>Kopyalayın ve ödeme sırasında kullanın!</p>
            </div>

            <div class='features'>
                <h3>✨ İndirim Avantajları</h3>
                <div class='feature-item'>✓ Tüm premium kurslarda geçerli</div>
                <div class='feature-item'>✓ Sınırsız erişim hakkı</div>
                <div class='feature-item'>✓ Sertifika imkanı</div>
                <div class='feature-item'>✓ Ömür boyu güncellemeler</div>
            </div>

            <p style='text-align: center;'>
                <a href='https://budemy.com/courses' class='button'>Kurslara Göz At</a>
            </p>

            <p style='background: #fff3cd; padding: 15px; border-radius: 5px; border-left: 4px solid #ffc107;'>
                ⚠️ <strong>Not:</strong> Bu indirim kodu sınırlı süre için geçerlidir. Kaçırmayın!
            </p>

            <p>İyi öğrenmeler dileriz! 🚀</p>
            <p><strong>Budemy Ekibi</strong></p>
        </div>
        <div class='footer'>
            <p>Bu otomatik bir e-postadır, lütfen yanıtlamayınız.</p>
            <p>© 2024 Budemy Platform. Tüm hakları saklıdır.</p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(email, subject, body);
        }
    }
}
