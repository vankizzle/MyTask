using CinemAPI.Domain.Contracts;
using CinemAPI.Domain.Contracts.Models;
using CinemAPI.Models;
using CinemAPI.Models.Input.Projection;
using CinemAPI.Data;
using System.Web.Http;
using CinemAPI.Data.Implementation;
using CinemAPI.Models.Contracts.Projection;
using System;
using CinemAPI.Models.Contracts.Room;
using CinemAPI.Models.Contracts.Cinema;
using CinemAPI.Models.Contracts.Movie;
using CinemAPI.Models.Contracts.ReservationTicket;

namespace CinemAPI.Controllers
{
    public class ProjectionController : ApiController
    {
        private readonly INewProjection newProj;
        private readonly IProjectionRepository projRepo;
        private readonly IRoomRepository roomRepo;
        private readonly ICinemaRepository cinemaRepo;
        private readonly IMovieRepository movieRepo;
        private readonly IReservationTicketRepository ReservationTicketRepo;

        public ProjectionController(INewProjection newProj, IProjectionRepository newProjRepo, IRoomRepository newRoomRepo, ICinemaRepository newCinemaRepo
            , IMovieRepository newMovieRepo, IReservationTicketRepository newReservationTicketRepo)
        {
            this.newProj = newProj;
            this.projRepo = newProjRepo;
            this.roomRepo = newRoomRepo;
            this.cinemaRepo = newCinemaRepo;
            this.movieRepo = newMovieRepo;
            this.ReservationTicketRepo = newReservationTicketRepo;
        }

        [HttpPost]
        public IHttpActionResult Index(ProjectionCreationModel model)
        {
            NewProjectionSummary summary = newProj.New(new Projection(model.MovieId, model.RoomId, model.StartDate));

            if (summary.IsCreated)
            {
                return Ok();
            }
            else
            {
                return BadRequest(summary.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult ProjectionAvailableSeatsCount(int projID)
        {
            IProjection tmp = projRepo.Get(projID);

            int result = DateTime.Compare(tmp.StartDate, DateTime.Now);

            if (result < 0)
            {
                return Ok(tmp.AvailableSeatsCount);
            }
            else
            {
                return BadRequest("Projection has already started");
            }
        }

        [HttpPost]
        public IHttpActionResult ReserveProjection(ProjectionReservationCreationModel model)
        {
            IProjection tmpproj = projRepo.Get(model.ProjectionID);

            IRoom tmproom = roomRepo.GetById(tmpproj.RoomId);

            ICinema tmpcinema = cinemaRepo.GetByID(tmproom.CinemaId);

            string moviename = projRepo.GetProjectionMovieName(model.ProjectionID);

            TimeSpan span = tmpproj.StartDate.Subtract(DateTime.Now);

            if (DateTime.Compare(tmpproj.StartDate, DateTime.Now) < 0 || span.TotalMinutes < 10)
            {
                return BadRequest("Can't reserve now");

            }
            else if (tmproom.Rows < model.ReservationRow)
            {
                return BadRequest("No such row");

            }
            else if (tmproom.SeatsPerRow < model.ReservationColumn)
            {
                return BadRequest("No such column");

            }
            else if (tmpproj.AvailableSeatsCount == 0)
            {
                return BadRequest("No free seats");

            }
            else if (ReservationTicketRepo.IsSeatReserved(model.ReservationRow, model.ReservationColumn))
            {
                return BadRequest("Seat is reserved");
            }
            else
            {
                IReservationTicketCreation resmodel = new ReservationTicket(tmpproj.StartDate, moviename, tmpcinema.Name, tmproom.Number, model.ReservationRow, model.ReservationColumn);
                ReservationTicketRepo.InsertReservation(resmodel);
                projRepo.DecreaseAvailableSeatCount(model.ProjectionID);
                return Ok(resmodel);
            }
        }

        [HttpPost]
        public IHttpActionResult BuyTicketWithoutReservation(ProjectionReservationCreationModel model)
        {
            ReservationTicketRepo.CalcelAllReservationIn10min(); //freeing seats

            var proj = projRepo.Get(model.ProjectionID);

            var room = roomRepo.GetById(proj.RoomId);

            var cinema = cinemaRepo.GetByID(room.CinemaId);

            var moviename = projRepo.GetProjectionMovieName(model.ProjectionID);


            if (DateTime.Compare(proj.StartDate, DateTime.Now) < 0 || DateTime.Compare(proj.StartDate, DateTime.Now) == 0)
            {
                return BadRequest("Projection already started,cant buy ticket");

            }
            else if (ReservationTicketRepo.IsSeatReserved(model.ReservationRow, model.ReservationColumn))
            {
                return BadRequest("Seat is reserved,cant buy ticket");
            }
            else
            {
                IReservationTicketCreation ticket = new ReservationTicket(proj.StartDate, moviename, cinema.Name, room.Number, model.ReservationRow, model.ReservationColumn);
                ReservationTicketRepo.InsertTicket(ticket);
                projRepo.DecreaseAvailableSeatCount(model.ProjectionID);
                return Ok(ticket);
            }
        }
        [HttpPost]
        public IHttpActionResult BuyTicketWithReservation(int ReservationID)
        {
            ReservationTicketRepo.CalcelAllReservationIn10min(); //freeing seats
            var reservation = ReservationTicketRepo.GetReservation(ReservationID);

            if(reservation == null)
            {
                return BadRequest("Reservation has expired");
            }
            else if (ReservationTicketRepo.TicketExists(ReservationID))
            {
                return BadRequest("Ticket already bought with this reservation ID");
            }
            else
            {
                var ticket = ReservationTicketRepo.ConvertReservationToTicket(ReservationID);
                ReservationTicketRepo.InsertTicket(ticket);
                return Ok(ticket);
            }
        }


    }
}