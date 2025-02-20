namespace SmartOffice.OfficeController
{
    public interface IFireAlarmManager
    {
        string GetStatus();
        void SetAlarm(bool state);
    }
}