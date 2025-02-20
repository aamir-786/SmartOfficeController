namespace SmartOffice.OfficeController
{
    public interface ILightManager
    {
        string GetStatus();
        void SetAllLights(bool state);
    }
}
