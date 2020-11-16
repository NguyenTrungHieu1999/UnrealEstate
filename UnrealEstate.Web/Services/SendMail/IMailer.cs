using System.Threading.Tasks;

namespace UnrealEstate.Web.Services.SendMail
{
    public interface IMailer
    {
        Task SenEmailAsync(string email, string subject, string body);
    }
}
