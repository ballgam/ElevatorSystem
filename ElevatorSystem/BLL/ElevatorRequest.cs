using ElevatorSystem.Enums;

namespace ElevatorSystem.BLL
{
    public class ElevatorRequest
    {

        private readonly int _currentFloor;
        private readonly int _destinationFloor;
        private readonly DirectionState _direction;

        public ElevatorRequest(int currentFloor, int destinationFLoor)
        {
            _currentFloor = currentFloor;
            _destinationFloor = destinationFLoor;
            _direction = destinationFLoor > currentFloor ? DirectionState.UP : DirectionState.DOWN;
        }

        public int CurrentFloor => _currentFloor;

        public int DestinationFloor => _destinationFloor;

        public DirectionState Direction => _direction;
    }

}
