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

namespace CinemAPI.Controllers
{
    public class ProjectionController : ApiController
    {
        private readonly INewProjection newProj;
        private readonly IProjectionRepository projRepo;
        private readonly IRoomRepository roomRepo;
        private readonly ICinemaRepository cinemaRepo;
        private readonly IMovieRepository movieRepo;
        private readonly IReservationTicketRepository ticketRepo;

        public ProjectionController(INewProjection newProj, IProjectionRepository newProjRepo, IRoomRepository newRoomRepo, ICinemaRepository newCinemaRepo
            , IMovieRepository newMovieRepo, IReservationTicketRepository newTicketRepo)
        {
            this.newProj = newProj;
            this.projRepo = newProjRepo;
            this.roomRepo = newRoomRepo;
            this.cinemaRepo = newCinemaRepo;
            this.movieRepo = newMovieRepo;
            this.ticketRepo = newTicketRepo;
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
        public IHttpActionResult AvailableSeatsCount(int projID)
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

            IMovie tmpmovie = movieRepo.GetById(tmpproj.MovieId);

            TimeSpan span = tmpproj.StartDate.Subtract(DateTime.Now);

            if (DateTime.Compare(tmpproj.StartDate, DateTime.Now) > 0 || span.TotalMinutes < 10)
            {
                return BadRequest("Can't reserve now");

            }else if(tmproom.Rows < model.ReservationRow)
            {
                return BadRequest("No such row");

            }else if(tmproom.SeatsPerRow < model.ReservationColumn)
            {
                return BadRequest("No such column");

            }else if(tmpproj.AvailableSeatsCount == 0)
            {
                return BadRequest("No free seats");

            }else if (ticketRepo.IsSeatReserved(model.ReservationRow, model.ReservationColumn))
            {
                return BadRequest("Seat is reserved");
            }
            else
            {
                ReservationTicket resmodel = new ReservationTicket(tmpproj.StartDate, tmpmovie.Name, tmpcinema.Name, tmproom.Number, model.ReservationRow, model.ReservationColumn);
                ticketRepo.Insert(resmodel);
                return Ok(resmodel);
            }


        }

        }
}