using CinemAPI.Models.Contracts.ReservationTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemAPI.Models
{
  public class ReservationTicket : IReservationTicket,IReservationTicketCreation
    {
        public ReservationTicket() { }

        public ReservationTicket(long ProjecitonID, DateTime projtime,string moviename,string cinemaname,int roomnum,int row,int col)
        {
            this.ProjectionID = ProjecitonID;
            this.ProjectionStartDate = projtime;
            this.MovieName = moviename;
            this.CinemaName = cinemaname;
            this.RoomNumber = roomnum;
            this.Row = row;
            this.Column = col;
        }

        public int ID { get; set; }

        public long ProjectionID { get; set; }

        public virtual Projection Projection { get; set; }

        public DateTime ProjectionStartDate { get; set; }

        public string MovieName { get; set; }

        public string CinemaName { get; set; }

        public int RoomNumber { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }
    }
}
