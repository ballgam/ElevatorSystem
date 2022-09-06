using ElevatorSystem.Enums;
using ElevatorSystem.Exceptions;
using ElevatorSystem.Interfaces;
using System.Security.Cryptography;

namespace ElevatorSystem.BLL
{
    public class ElevatorController : IElevatorController
    {
        private readonly int _weightCapacity;
        private readonly List<Elevator> _elevators;
        public List<Elevator> Elevators => _elevators;

        public int FloorCount => _floorCount;

        private readonly int _floorCount;
        private readonly Dictionary<ElevatorRequest, int> _assignedRequests = new();

        public ElevatorController(int elevatorCount, int floorCount, int weightCapacity = 10)
        {
            _floorCount = floorCount;
            _weightCapacity = weightCapacity;
            _elevators = new List<Elevator>(elevatorCount);
            _assignedRequests = new Dictionary<ElevatorRequest, int>();

            InitElevators(elevatorCount);
        }

        /// <summary>
        /// Places the elevators on random floors and assigns random travel direction and random weight load
        /// </summary>
        public void RandomizeElevatorFloorSelection()
        {
            var random = new Random();

            Array directions = Enum.GetValues(typeof(DirectionState));

            foreach (var elevator in _elevators)
            {
                DirectionState direction = (DirectionState)directions.GetValue(random.Next(directions.Length)); ;
                elevator.Assign(random.Next(_floorCount), direction, random.Next(_weightCapacity));
            }
        }

        /// <summary>
        /// Initializes all the elevators
        /// </summary>
        /// <param name="elevatorCount"></param>
        private void InitElevators(int elevatorCount)
        {
            for (int i = 0; i < elevatorCount; i++)
            {
                _elevators.Add(Elevator.Init(i));
            }
        }

        /// <summary>
        /// Processes the request by first checking if an identical request has been made. If not, it finds the nearest elevator from the requested floor
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Elevator to serve the request</returns>
        public Elevator ProcessRequest(ElevatorRequest request)
        {
            int? elevatorIndex = null;

            // Check if request on floor is already assigned to elevator
            if (_assignedRequests.ContainsKey(request))
            {
                elevatorIndex = _assignedRequests[request];

                // Check weight capacity
                if (_elevators.ElementAt(elevatorIndex.Value).CurrentWeightCapacity != _weightCapacity)
                {
                    _elevators.ElementAt(elevatorIndex.Value).CurrentWeightCapacity += 1;
                }
                else
                {
                    elevatorIndex = null;
                }
            }

            // Look for the nearest elevator travelling the same direction as requested by the user
            if (elevatorIndex is null)
            {
                elevatorIndex = GetNearestElevator(request);

                _elevators.ElementAt(elevatorIndex.Value).CurrentWeightCapacity += 1;
                _elevators.ElementAt(elevatorIndex.Value).CurrentFloor = request.DestinationFloor;
                _assignedRequests[request] = elevatorIndex.Value;
            }

            return _elevators[elevatorIndex.Value];
        }

        /// <summary>
        /// Gets the nearest elevator based on the request floor
        /// </summary>
        /// <param name="request"></param>
        /// <returns>index of the Elevator</returns>
        private int GetNearestElevator(ElevatorRequest request)
        {
            PriorityQueue<int, int> queue = new PriorityQueue<int, int>(1);

            for (int i = 0; i < _elevators.Count; i++)
            {
                Elevator? elevator = _elevators[i];

                if (elevator.DirectionState == request.Direction && elevator.CurrentWeightCapacity != _weightCapacity)
                {
                    int variance = -1;
                    if (request.Direction == DirectionState.UP)
                        variance = request.CurrentFloor - elevator.CurrentFloor;
                    else
                        variance = elevator.CurrentFloor - request.CurrentFloor;

                    if (variance >= 0)
                        queue.Enqueue(i, request.CurrentFloor - elevator.CurrentFloor);
                }
            }

            if (queue.Count == 0)
            {
                // There are no elevators with capacity travelling the same direction as the user
                // Get the nearest elevator completing a round trip
                var oppDirectionElevators = _elevators.Where(el => el.DirectionState != request.Direction);

                for (int i = 0; i < _elevators.Count; i++)
                {
                    Elevator? elevator = _elevators[i];

                    if (elevator.DirectionState != request.Direction && elevator.CurrentWeightCapacity != _weightCapacity)
                    {
                        // Enqueue the nearest elevator with capacity

                        if (elevator.DirectionState == DirectionState.UP)
                            queue.Enqueue(i, (_floorCount - elevator.CurrentFloor) + (_floorCount - request.CurrentFloor));
                        else
                            queue.Enqueue(i, request.CurrentFloor + elevator.CurrentFloor);
                    }
                }

            }

            return queue.Dequeue();
        }

        /// <summary>
        /// Removes the request from the queue
        /// </summary>
        /// <param name="request"></param>
        public void RemoveRequest(ElevatorRequest request)
        {
            if (_assignedRequests.ContainsKey(request))
            {
                var index = _assignedRequests[request];
                _elevators.ElementAt(index).CurrentWeightCapacity -= 1;
            }
        }
    }

}
