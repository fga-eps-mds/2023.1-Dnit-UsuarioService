using service.Interfaces;
using System.Net.Mail;
using System.Net;
using System;

namespace service
{
    public class EmailService : IEmailService
    {
        public void EnviarEmail(string emailDestinatario, string assunto, string corpo)
        {

            MailMessage mensagem = new MailMessage();

            string emailRemetente = Environment.GetEnvironmentVariable("EMAIL_SERVICE_ADDRESS");
            string senhaRemetente = Environment.GetEnvironmentVariable("EMAIL_SERVICE_PASSWORD");

            mensagem.From = new MailAddress(emailRemetente);
            mensagem.Subject = assunto;
            mensagem.To.Add(new MailAddress(emailDestinatario));
            mensagem.Body = corpo;

            var clienteSmtp = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(emailRemetente, senhaRemetente),
                EnableSsl = true,

            };
            clienteSmtp.Send(mensagem);
        }
    }
}
