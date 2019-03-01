using CinemAPI.Models.Contracts.Projection;
using System;
using System.ComponentModel.DataAnnotations;

namespace CinemAPI.Models
{
    public class Projection : IProjection, IProjectionCreation
    {
        public Projection()
        {
        }

        public Projection(int movieId, int roomId, DateTime startdate)
        {
            this.MovieId = movieId;
            this.RoomId = roomId;
            this.StartDate = startdate;
        }

        public long Id { get; set; }

        public int RoomId { get; set; }

        public virtual Room Room { get; set; }

        public int MovieId { get; set; }
        
        public virtual Movie Movie { get; set; }

        public DateTime StartDate { get; set; }

        [Range(0,int.MaxValue)]
        public int AvailableSeatsCount { get; set; } 
    }
}