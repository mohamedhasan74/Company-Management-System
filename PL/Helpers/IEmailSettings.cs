using DAL.Models;

namespace PL.Helpers
{
    public interface IEmailSettings
    {
        public void SendEmail(Email email);
    }
}
