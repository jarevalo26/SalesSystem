using SalesSystem.Application.Interfaces;
using SalesSystem.Domain.Entities;
using System.Net;
using System.Net.Mail;

namespace SalesSystem.Application.Implementation
{
    public class MailService : IMailService
    {
        private readonly IGenericRepository<Configuracion> _repository;

        public MailService(IGenericRepository<Configuracion> repository)
        {
            _repository = repository;
        }

        public async Task<bool> SendEmail(string mailFor, string mailSubject, string message)
        {
            try
            {
                IQueryable<Configuracion> query = await _repository.GetAll(c => c.Recurso!.Equals("Servicio_Correo"));
                Dictionary<string, string> config = query.ToDictionary(keySelector: c => c.Propiedad!, elementSelector: c => c.Valor!);
                var credentials = new NetworkCredential(config["correo"], config["clave"]);
                MailMessage mail = new()
                {
                    From = new MailAddress(config["correo"], config["alias"]),
                    Subject = mailSubject,
                    Body = message,
                    IsBodyHtml = true
                };

                mail.To.Add(new MailAddress(mailFor));
                var smtp = new SmtpClient()
                {
                    Host = config["host"],
                    Port = int.Parse(config["puerto"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true
                };

                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
