using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaBooking.Rooms
{
    interface IRoom
    {
        RoomType type { get; set; }
    }
    public enum RoomType
    {
        Boardroom, Classroom, Theatre
    }
}
