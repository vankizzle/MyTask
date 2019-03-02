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

        public IReservationTicketCreation ConvertReservationToTicket(int ReservationID)
        {
            IReservationTicket reservation = GetReservation(ReservationID);

            IReservationTicketCreation ticket = new ReservationTicket(reservation.ProjectionStartDate, reservation.MovieName,
            reservation.CinemaName, reservation.RoomNumber, reservation.Row, reservation.Column);

            db.Reservations.Remove((ReservationTicket)reservation);
            db.SaveChanges();

            return ticket;
        }

        public void CalcelAllReservationIn10min()
        {
            foreach (ReservationTicket reservation in db.Reservations)
            {
                TimeSpan span = reservation.ProjectionStartDate.Subtract(DateTime.Now);

                if (span.TotalMinutes <= 10)
                {
                    CancelReservation(reservation.ID);
                    var movieID = db.Movies.FirstOrDefault(x => x.Name.Equals(reservation.MovieName)).Id;
                    var roomID = db.Rooms.FirstOrDefault(x => x.Number == reservation.RoomNumber).Id;
                    ProjectionRepository projRepo = new ProjectionRepository(db);
                    var projection = projRepo.Get(movieID, roomID, reservation.ProjectionStartDate);
                    projRepo.IncreaseAvailableSeatCount(projection.Id);
                }
            }

            db.SaveChanges();
        }

        public bool CancelReservation(int ReservationID)
        {
            var entity = GetReservation(ReservationID);
            if(entity != null)
            {
                db.Reservations.Remove((ReservationTicket)entity);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
          
        }

        public IReservationTicket GetReservation(int ID)
        {
            return db.Reservations.FirstOrDefault(x => x.ID == ID);
        }

        public IReservationTicket GetTicket(int ID)
        {
            return db.Tickets.FirstOrDefault(x => x.ID == ID);
        }

        public void InsertReservation(IReservationTicketCreation resticket)
        {
            ReservationTicket newTicket = new ReservationTicket(resticket.ProjectionStartDate, resticket.MovieName, resticket.CinemaName, resticket.RoomNumber, resticket.Row, resticket.Column);

            db.Reservations.Add(newTicket);
            db.SaveChanges();
        }

        public void InsertTicket(IReservationTicketCreation ticket)
        {
            ReservationTicket newTicket = new ReservationTicket(ticket.ProjectionStartDate, ticket.MovieName, ticket.CinemaName, ticket.RoomNumber, ticket.Row, ticket.Column);
            db.Tickets.Add(newTicket);
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

        public bool TicketExists(int ReservationID)
        {
            var tmp = db.Tickets.FirstOrDefault(x => x.ID == ReservationID);

            if (tmp == null)
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
