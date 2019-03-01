using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemAPI.Models.Input.Projection
{
    public class ReservationTicketCreationModel
    {
        public DateTime ProjectionStartDate { get; set; }
        public string MovieName { get; set; }
        public string CinemaName { get; set; }
        public int RoomNumber { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}