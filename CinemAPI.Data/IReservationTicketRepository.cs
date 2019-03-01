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

        void Insert(IReservationTicketCreation resticket);

        IReservationTicket Get(int ID);

        bool IsSeatReserved(int row, int col);
    }
}
