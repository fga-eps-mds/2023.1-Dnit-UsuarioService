namespace service.Interfaces
{
    public interface IEmailService
    {
        public void EnviarEmail(string emailDestinatario, string assunto, string corpo);
    }
}
