using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace VismaBooking.Rooms
{
    public class Room:IRoom
    {
        private short _id;
        public short id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        private Image _picture;
        public Image picture
        {
            get
            {
                return _picture;
            }
            set
            {
                _picture = value;
            }
        }
        private string _equipment;
        public string equipment
        {
            get
            {
                return _equipment;
            }
            set
            {
                _equipment = value;
            }
        }
        private RoomType _type;
        public RoomType type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        private short _max_capacity;
        public short max_capacity
        {
            get
            {
                return _max_capacity;
            }
            set
            {
                _max_capacity = value;
            }
        }
        private short _floor;
        public short floor
        {
            get
            {
                return _floor;
            }
            set
            {
                _floor = value;
            }
        }
        private bool _booked;
        public bool booked
        {
            get
            {
                return _booked;
            }
            set
            {
                _booked = value;
            }
        }
        private string _booked_by;
        public string booked_by
        {
            get
            {
                return _booked_by;
            }
            set
            {
                _booked_by = value;
            }
        }
        private DateTime _booked_at;
        public DateTime booked_at
        {
            get
            {
                return _booked_at;
            }
            set
            {
                _booked_at = value;
            }
        }
        private DateTime _meeting_start;
        public DateTime meeting_start
        {
            get
            {
                return _meeting_start;
            }
            set
            {
                _meeting_start = value;
            }
        }
        private DateTime _meeting_end;
        public DateTime meeting_end
        {
            get
            {
                return _meeting_end;
            }
            set
            {
                _meeting_end = value;
            }
        }
    }
}
