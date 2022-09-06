using ElevatorSystem.BLL;

namespace ElevatorSystem.Interfaces
{
    public interface IElevatorController
    {
        List<Elevator> Elevators { get; }
        int FloorCount { get; }

        Elevator ProcessRequest(ElevatorRequest request);
        void RandomizeElevatorFloorSelection();
        void RemoveRequest(ElevatorRequest request);
    }
}