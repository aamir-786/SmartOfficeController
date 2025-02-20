namespace SmartOffice.OfficeController
{
    public interface IDoorManager
    {
        string GetStatus();
        bool OpenAllDoors();
        bool LockAllDoors();
    }
}
