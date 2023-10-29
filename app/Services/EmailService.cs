using app.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using System;

namespace app.Services
{
    public class EmailService : IEmailService
    {
        public void EnviarEmail(string emailDestinatario, string assunto, string corpo)
        {
            MailMessage mensagem = new MailMessage();

            string emailRemetente = DotNetEnv.Env.GetString("EMAIL_SERVICE_ADDRESS");
            string senhaRemetente = DotNetEnv.Env.GetString("EMAIL_SERVICE_PASSWORD");

            mensagem.From = new MailAddress(emailRemetente);
            mensagem.Subject = assunto;
            mensagem.To.Add(new MailAddress(emailDestinatario));
            mensagem.Body = corpo;

            var enderecoSmtp = DotNetEnv.Env.GetString("EMAIL_SERVICE_SMTP") ?? "smtp-mail.outlook.com";
            var clienteSmtp = new SmtpClient(enderecoSmtp)
            {
                Port = 587,
                Credentials = new NetworkCredential(emailRemetente, senhaRemetente),
                EnableSsl = true,

            };

            clienteSmtp.Send(mensagem);
        }
    }
}
