using ElevatorSystem;
using ElevatorSystem.BLL;
using ElevatorSystem.Enums;
using ElevatorSystem.Interfaces;
using Spectre.Console;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

AnsiConsole.Write(
    new FigletText("Utilita Elevator")
    .Centered()
    .Color(Color.Green));


var settings = DvtConsole.PromptInitialSettings();

IElevatorController controller = new ElevatorController(elevatorCount: settings.elevatorCount,
                                                        floorCount: settings.floorCount,
                                                        weightCapacity: settings.weightCapacity);


void MakeElevatorRequest(IElevatorController controller)
{
REQUEST_START:
    var usrReq = DvtConsole.PromptRequestFromUser(controller);

    var req = new ElevatorRequest(usrReq.currentFloor, usrReq.destinationFloor);

    var elevator = controller.ProcessRequest(req);

    DvtConsole.DisplayElevatorDetails(elevator);

    DvtConsole.DisplayElevatorTableAttributes(controller);

    var selection = DvtConsole.Prompt<Options>("Input new request?");
    if (selection == Options.YES)
        goto REQUEST_START;
}



ELEVATOR_SETUP:

AnsiConsole.WriteLine();
AnsiConsole.Write(new Rule($"[yellow]Randomizing floor on which the elevator is on[/]").RuleStyle("grey").LeftAligned());

controller.RandomizeElevatorFloorSelection();
DvtConsole.DisplayElevatorTableAttributes(controller);

var selection = DvtConsole.Prompt<Options>("Request for elevator with the current setup?");

if (selection == Options.YES)
{
    MakeElevatorRequest(controller);
}
else
{
    goto ELEVATOR_SETUP;
}
















