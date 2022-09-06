using ElevatorSystem.BLL;
using ElevatorSystem.Enums;
using ElevatorSystem.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem
{
    public static class DvtConsole
    {
        public static (int floorCount, int elevatorCount, int weightCapacity) PromptInitialSettings()
        {
            int floorCount = AnsiConsole.Prompt(new TextPrompt<int>("How many Floors does the building have?"));
            int elevatorCount = AnsiConsole.Prompt(new TextPrompt<int>("How many elevators are in the building?"));

            int weightCapacity = AnsiConsole.Prompt(new TextPrompt<int>("Whats the weight capacity for a single elevator (In number of people)?"));

            return (floorCount, elevatorCount, weightCapacity);
        }

        public static void DisplayElevatorTableAttributes(IElevatorController controller)
        {
            Table table = new Table()
                .AddColumn(new TableColumn("Elevator").Centered())
                .AddColumn(new TableColumn("Floor").Centered())
                .AddColumn(new TableColumn("Direction").Centered())
                .AddColumn(new TableColumn("Current Capacity").Centered());

            for (int i = 0; i < controller.Elevators.Count; i++)
            {
                var elevator = controller.Elevators[i];
                string direction = elevator.DirectionState.ToString();
                table.AddRow($"Elevator {i}", $"{elevator.CurrentFloor}", direction, $"{elevator.CurrentWeightCapacity}");
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule($"[yellow]Elevators Details[/]").RuleStyle("grey").LeftAligned());
            AnsiConsole.Write(table);
        }

        internal static void DisplayElevatorDetails(Elevator elevator)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule($"[green]Boarded the Elevator below[/]").RuleStyle("grey").LeftAligned());
            string direction = elevator.DirectionState.ToString();
            AnsiConsole.Write(new Table()
                    .AddColumn(new TableColumn("Elevator").Centered())
                    .AddColumn(new TableColumn("Current Floor").Centered())
                    .AddColumn(new TableColumn("Direction").Centered())
                    .AddColumn(new TableColumn("Current Capacity").Centered())
                    .AddRow($"Elevator {elevator.ElevatorIndex}", $"{elevator.CurrentFloor}", direction, $"{elevator.CurrentWeightCapacity}"));
        }

        internal static T Prompt<T>(string title)
        {
            T[] enums = (T[])Enum.GetValues(typeof(T));

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<T>()
                .Title(title)
                .AddChoices(enums));

            return selection;
        }

        public static (int currentFloor, int destinationFloor) PromptRequestFromUser(IElevatorController controller)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule($"[yellow]Elevator Request[/]").RuleStyle("grey").LeftAligned());

            int curFloor = AnsiConsole.Prompt(new TextPrompt<int>("Which floor are you on?")
                                        .Validate(cnt
                                            => cnt <= controller.FloorCount ? ValidationResult.Success()
                                            : ValidationResult.Error("[red]Floor number shoud not exceed the number of floors in the building[/]")));

            int destFloor = AnsiConsole.Prompt(new TextPrompt<int>("Which floor do you want to go to?")
                                        .Validate(cnt
                                            => cnt <= controller.FloorCount ? ValidationResult.Success()
                                            : ValidationResult.Error("[red]Floor number shoud not exceed the number of floors in the building[/]")));

            return (curFloor, destFloor);
        }
    }
}
