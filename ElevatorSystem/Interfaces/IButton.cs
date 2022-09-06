using ElevatorSystem.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Interfaces
{
       public interface IButton
    {
        void PlaceRequest(ElevatorRequest request);
    }
}
