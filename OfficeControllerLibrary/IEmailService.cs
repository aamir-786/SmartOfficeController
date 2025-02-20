namespace SmartOffice.OfficeController
{
    public interface IEmailService
    {
        void SendMail(string to, string subject, string message);
    }
}