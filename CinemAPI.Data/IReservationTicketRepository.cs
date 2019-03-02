using CinemAPI.Models.Contracts.ReservationTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemAPI.Data
{
    public interface IReservationTicketRepository
    {

        void InsertReservation(IReservationTicketCreation resticket);

        void InsertTicket(IReservationTicketCreation resticket);

        IReservationTicket GetReservation(int ID);

        IReservationTicket GetTicket(int ID);

        bool IsSeatReserved(int row, int col);

        bool CancelReservation(int ReservationID);

        void CalcelAllReservationIn10min();

        bool TicketExists(int ID);

        IReservationTicketCreation ConvertReservationToTicket(int ReservationID);
    }
}
