namespace ElevatorSystem.Exceptions
{
    public class NoAvailableElevatorException : Exception
    {
        public NoAvailableElevatorException()
        {
        }

        public NoAvailableElevatorException(string message)
            : base(message)
        {
        }

        public NoAvailableElevatorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
