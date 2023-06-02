using service.Interfaces;
using System.Net.Mail;
using System.Net;

namespace service
{
    public class EmailService : IEmailService
    {
        public void EnviarEmail(string emailDestinatario, string assunto, string corpo)
        {

            MailMessage mensagem = new MailMessage();

            string emailRemetente = "email@gmail.com";
            string senhaRemetente = "senha";

            mensagem.From = new MailAddress(emailRemetente);
            mensagem.Subject = assunto;
            mensagem.To.Add(new MailAddress(emailDestinatario));
            mensagem.Body = corpo;

            var clienteSmtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(emailRemetente, senhaRemetente),
                EnableSsl = true,

            };
            clienteSmtp.Send(mensagem);
        }
    }
}
