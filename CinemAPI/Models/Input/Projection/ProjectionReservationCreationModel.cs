using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemAPI.Models.Input.Projection
{
    public class ProjectionReservationCreationModel
    {
        public int ProjectionID { get; set; }

        public int ReservationRow { get; set; }

        public int ReservationColumn { get; set; }
    }
}