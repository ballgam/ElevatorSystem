using ElevatorSystem.Enums;
using System;

namespace ElevatorSystem.BLL
{
    public class Elevator
    {
        public Elevator(int elevatorIndex)
        {
            ElevatorIndex = elevatorIndex;
        }

        public int ElevatorIndex { get; set; }
        public int CurrentWeightCapacity { get; set; }
        public int CurrentFloor { get; set; }

        public DirectionState DirectionState { get; set; }

        public ElevatorState ElevatorState { get; set; }

        public static Elevator Init(int index)
        {
            return new Elevator(index);
        }

        internal void Assign(int floorCount, DirectionState direction, int weight)
        {
            CurrentFloor = floorCount;
            DirectionState = direction;
            CurrentWeightCapacity = weight;
        }
    }

}
