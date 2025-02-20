namespace SmartOffice.OfficeController
{
    public interface IWebService
    {
        void LogFireAlarm(string message);
        void LogEngineerRequired(string message);
    }
}