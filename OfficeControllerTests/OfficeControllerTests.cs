using NUnit.Framework;
using NSubstitute;

[TestFixture]
public class OfficeControllerTests
{
    [Test]
    public void L2R1_AdditionalConstructorSetsInitialState()
    {
        // Arrange
        var officeId = "TestOffice";
        var startState = "Open";

        // Act
        var controller = new OfficeController(officeId, startState);

        // Assert
        Assert.AreEqual(officeId.ToLower(), controller.GetOfficeID());
        Assert.AreEqual(startState.ToLower(), controller.GetCurrentState());
    }

    [Test]
    public void L2R1_AdditionalConstructorThrowsExceptionForInvalidState()
    {
        // Arrange
        var officeId = "TestOffice";
        var startState = "InvalidState";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new OfficeController(officeId, startState));
    }

    [Test]
    public void L2R2_DependencyInjectionConstructor()
    {
        // Arrange
        var officeId = "TestOffice";
        var lightManager = Substitute.For<ILightManager>();
        var fireAlarmManager = Substitute.For<IFireAlarmManager>();
        var doorManager = Substitute.For<IDoorManager>();
        var webService = Substitute.For<IWebService>();
        var emailService = Substitute.For<IEmailService>();

        // Act
        var controller = new OfficeController(officeId, lightManager, fireAlarmManager, doorManager, webService, emailService);

        // Assert
        Assert.AreEqual(officeId.ToLower(), controller.GetOfficeID());
        Assert.AreEqual("out of hours", controller.GetCurrentState());
    }

    [Test]
    public void L2R3_GetStatusReportCombinesStatuses()
    {
        // Arrange
        var officeId = "TestOffice";
        var lightManager = Substitute.For<ILightManager>();
        var fireAlarmManager = Substitute.For<IFireAlarmManager>();
        var doorManager = Substitute.For<IDoorManager>();
        var webService = Substitute.For<IWebService>();
        var emailService = Substitute.For<IEmailService>();

        lightManager.GetStatus().Returns("Lights,OK,OK,FAULT");
        doorManager.GetStatus().Returns("Doors,OK,OK,OK");
        fireAlarmManager.GetStatus().Returns("FireAlarm,OK,OK");

        var controller = new OfficeController(officeId, lightManager, fireAlarmManager, doorManager, webService, emailService);

        // Act
        var statusReport = controller.GetStatusReport();

        // Assert
        Assert.AreEqual("Lights,OK,OK,FAULT,Doors,OK,OK,OK,FireAlarm,OK,OK", statusReport);
    }

    [Test]
    public void L2R4_SetCurrentStateToOpenFailsIfDoorsCannotBeOpened()
    {
        // Arrange
        var officeId = "TestOffice";
        var lightManager = Substitute.For<ILightManager>();
        var fireAlarmManager = Substitute.For<IFireAlarmManager>();
        var doorManager = Substitute.For<IDoorManager>();
        var webService = Substitute.For<IWebService>();
        var emailService = Substitute.For<IEmailService>();

        doorManager.OpenAllDoors().Returns(false);

        var controller = new OfficeController(officeId, lightManager, fireAlarmManager, doorManager, webService, emailService);

        // Act
        var result = controller.SetCurrentState("open");

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual("out of hours", controller.GetCurrentState());
    }

    [Test]
    public void L2R5_SetCurrentStateToOpenSucceedsIfDoorsAreOpened()
    {
        // Arrange
        var officeId = "TestOffice";
        var lightManager = Substitute.For<ILightManager>();
        var fireAlarmManager = Substitute.For<IFireAlarmManager>();
        var doorManager = Substitute.For<IDoorManager>();
        var webService = Substitute.For<IWebService>();
        var emailService = Substitute.For<IEmailService>();

        doorManager.OpenAllDoors().Returns(true);

        var controller = new OfficeController(officeId, lightManager, fireAlarmManager, doorManager, webService, emailService);

        // Act
        var result = controller.SetCurrentState("open");

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual("open", controller.GetCurrentState());
    }

    [Test]
    public void L3R1_SetCurrentStateToClosedLocksDoorsAndTurnsOffLights()
    {
        // Arrange
        var officeId = "TestOffice";
        var lightManager = Substitute.For<ILightManager>();
        var fireAlarmManager = Substitute.For<IFireAlarmManager>();
        var doorManager = Substitute.For<IDoorManager>();
        var webService = Substitute.For<IWebService>();
        var emailService = Substitute.For<IEmailService>();

        var controller = new OfficeController(officeId, lightManager, fireAlarmManager, doorManager, webService, emailService);

        // Act
        controller.SetCurrentState("closed");

        // Assert
        doorManager.Received().LockAllDoors();
        lightManager.Received().SetAllLights(false);
        Assert.AreEqual("closed", controller.GetCurrentState());
    }

    [Test]
    public void L3R2_SetCurrentStateToFireAlarmTriggersAlarmAndLogs()
    {
        // Arrange
        var officeId = "TestOffice";
        var lightManager = Substitute.For<ILightManager>();
        var fireAlarmManager = Substitute.For<IFireAlarmManager>();
        var doorManager = Substitute.For<IDoorManager>();
        var webService = Substitute.For<IWebService>();
        var emailService = Substitute.For<IEmailService>();

        var controller = new OfficeController(officeId, lightManager, fireAlarmManager, doorManager, webService, emailService);

        // Act
        controller.SetCurrentState("fire alarm");

        // Assert
        fireAlarmManager.Received().SetAlarm(true);
        doorManager.Received().OpenAllDoors();
        lightManager.Received().SetAllLights(true);
        webService.Received().LogFireAlarm("fire alarm");
        Assert.AreEqual("fire alarm", controller.GetCurrentState());
    }

    [Test]
    public void L3R3_GetStatusReportLogsFaults()
    {
        // Arrange
        var officeId = "TestOffice";
        var lightManager = Substitute.For<ILightManager>();
        var fireAlarmManager = Substitute.For<IFireAlarmManager>();
        var doorManager = Substitute.For<IDoorManager>();
        var webService = Substitute.For<IWebService>();
        var emailService = Substitute.For<IEmailService>();

        lightManager.GetStatus().Returns("Lights,OK,OK,FAULT");
        doorManager.GetStatus().Returns("Doors,OK,OK,OK");
        fireAlarmManager.GetStatus().Returns("FireAlarm,OK,OK");

        var controller = new OfficeController(officeId, lightManager, fireAlarmManager, doorManager, webService, emailService);

        // Act
        var statusReport = controller.GetStatusReport();

        // Assert
        webService.Received().LogEngineerRequired("Lights");
        Assert.AreEqual("Lights,OK,OK,FAULT,Doors,OK,OK,OK,FireAlarm,OK,OK", statusReport);
    }

    [Test]
    public void L3R4_SetCurrentStateToInvalidStateReturnsFalse()
    {
        // Arrange
        var officeId = "TestOffice";
        var lightManager = Substitute.For<ILightManager>();
        var fireAlarmManager = Substitute.For<IFireAlarmManager>();
        var doorManager = Substitute.For<IDoorManager>();
        var webService = Substitute.For<IWebService>();
        var emailService = Substitute.For<IEmailService>();

        var controller = new OfficeController(officeId, lightManager, fireAlarmManager, doorManager, webService, emailService);

        // Act
        var result = controller.SetCurrentState("invalid state");

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual("out of hours", controller.GetCurrentState());
    }

//L3R4: In addition to requirement L3R2, If WebService.LogFireAlarm( ) throws an Exception when called, an email should be sent using the EmailService’s SendMail( ) method to citycouncil@preston.gov.uk, with the subject “failed to log alarm” and the message parameter should contain the exception message returned from the failed call to the LogFireAlarm() function.
//EmailService’s SendMail( ) method to citycouncil@preston.gov.uk, with the subject “failed to log alarm” and the message parameter should contain the exception message returned from the failed call to the LogFireAlarm() function.

[Test]
public void L3R4_SetCurrentStateToFireAlarmLogsFaultsAndSendsEmailOnWebServiceException()
{
    // Arrange
    var officeId = "TestOffice";
    var lightManager = Substitute.For<ILightManager>();
    var fireAlarmManager = Substitute.For<IFireAlarmManager>();
    var doorManager = Substitute.For<IDoorManager>();
    var webService = Substitute.For<IWebService>();
    var emailService = Substitute.For<IEmailService>();

    webService.When(x => x.LogFireAlarm("fire alarm")).Do(x => { throw new Exception("Failed to log alarm"); });

    var controller = new OfficeController(officeId, lightManager, fireAlarmManager, doorManager, webService, emailService);

    // Act
    controller.SetCurrentState("fire alarm");

    // Assert
    fireAlarmManager.Received().SetAlarm(true);
    doorManager.Received().OpenAllDoors();
    lightManager.Received().SetAllLights(true);
    webService.Received().LogFireAlarm("fire alarm");
    emailService.Received().SendMail("citycouncil@preston.gov.uk", "failed to log alarm", "Failed to log alarm");
    Assert.AreEqual("fire alarm", controller.GetCurrentState());

}
    
}