using System;

// Here are the Interfaces and Classes that are used in the OfficeController class
public interface ILightManager
{
    bool SetAllLights(bool state);
    string GetStatus();
}

public interface IDoorManager
{
    bool OpenAllDoors();
    bool LockAllDoors();
    string GetStatus();
}

public interface IFireAlarmManager
{
    void SetAlarm(bool state);
    string GetStatus();
}

public interface IWebService
{
    void LogFireAlarm(string message);
    void LogEngineerRequired(string message);
}

public interface IEmailService
{
    void SendMail(string to, string subject, string message);
}

public class OfficeController
{
    private string officeID;
    private string currentState;
    private ILightManager lightManager;
    private IDoorManager doorManager;
    private IFireAlarmManager fireAlarmManager;
    private IWebService webService;
    private IEmailService emailService;

    public OfficeController(string id)
    {
        officeID = id.ToLower();
        currentState = "out of hours";
        lightManager = null;
        doorManager = null;
        fireAlarmManager = null;
        webService = null;
        emailService = null;
    }

    public OfficeController(string id, string startState)
    {
        officeID = id.ToLower();
        if (new List<string> { "closed", "out of hours", "open" }.Contains(startState.ToLower()))
        {
            currentState = startState.ToLower();
        }
        else
        {
            throw new ArgumentException("Argument Exception: OfficeController can only be initialised to the following states 'open', 'closed', 'out of hours'");
        }
        lightManager = null;
        doorManager = null;
        fireAlarmManager = null;
        webService = null;
        emailService = null;
    }

    public OfficeController(string id, ILightManager iLightManager, IFireAlarmManager iFireAlarmManager, IDoorManager iDoorManager, IWebService iWebService, IEmailService iEmailService)
    {
        officeID = id.ToLower();
        currentState = "out of hours";
        lightManager = iLightManager;
        doorManager = iDoorManager;
        fireAlarmManager = iFireAlarmManager;
        webService = iWebService;
        emailService = iEmailService;
    }

    public string GetOfficeID()
    {
        return officeID;
    }

    public void SetOfficeID(string id)
    {
        officeID = id.ToLower();
    }

    public string GetCurrentState()
    {
        return currentState;
    }

    public bool SetCurrentState(string newState)
    {
        var validStates = new List<string> { "closed", "out of hours", "open", "fire drill", "fire alarm" };
        newState = newState.ToLower();

        if (!validStates.Contains(newState))
        {
            return false;
        }

        switch (newState)
        {
            case "open":
                if (!doorManager.OpenAllDoors())
                {
                    return false;
                }
                break;
            case "closed":
                doorManager.LockAllDoors();
                lightManager.SetAllLights(false);
                break;
            case "fire alarm":
                fireAlarmManager.SetAlarm(true);
                doorManager.OpenAllDoors();
                lightManager.SetAllLights(true);
                try
                {
                    webService.LogFireAlarm("fire alarm");
                }
                catch (Exception ex)
                {
                    emailService.SendMail("citycouncil@preston.gov.uk", "failed to log alarm", ex.Message);
                }
                break;
        }

        currentState = newState;
        return true;
    }

    public string GetStatusReport()
    {
        var lightStatus = lightManager.GetStatus();
        var doorStatus = doorManager.GetStatus();
        var fireAlarmStatus = fireAlarmManager.GetStatus();

        if (lightStatus.Contains("FAULT") || doorStatus.Contains("FAULT") || fireAlarmStatus.Contains("FAULT"))
        {
            var faultyDevices = new List<string>();
            if (lightStatus.Contains("FAULT")) faultyDevices.Add("Lights");
            if (doorStatus.Contains("FAULT")) faultyDevices.Add("Doors");
            if (fireAlarmStatus.Contains("FAULT")) faultyDevices.Add("FireAlarm");

            webService.LogEngineerRequired(string.Join(",", faultyDevices));
        }

        return $"{lightStatus},{doorStatus},{fireAlarmStatus}";
    }
}