using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemAPI.Data.EF;
using CinemAPI.Models;
using CinemAPI.Models.Contracts.ReservationTicket;

namespace CinemAPI.Data.Implementation
{
    public class ReservationTicketRepository : IReservationTicketRepository
    {
        private readonly CinemaDbContext db;

        public ReservationTicketRepository(CinemaDbContext db)
        {
            this.db = db;
        }

        public IReservationTicket Get(int ID)
        {
            return db.Reservations.FirstOrDefault(x => x.ID == ID);
        }

        public void Insert(IReservationTicketCreation resticket)
        {
            ReservationTicket newTicket = new ReservationTicket(resticket.ProjectionStartDate, resticket.MovieName, resticket.CinemaName, resticket.RoomNumber, resticket.Row, resticket.Column);

            db.Reservations.Add(newTicket);
            db.SaveChanges();
        }

        public bool IsSeatReserved(int row, int col)
        {
            var a = db.Reservations.FirstOrDefault(x => x.Row == row && x.Column == col);
            if(a == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
