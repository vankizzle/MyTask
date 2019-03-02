using CinemAPI.Data.EF;
using CinemAPI.Models;
using CinemAPI.Models.Contracts.Projection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemAPI.Data.Implementation
{
    public class ProjectionRepository : IProjectionRepository
    {
        private readonly CinemaDbContext db;

        public ProjectionRepository(CinemaDbContext db)
        {
            this.db = db;
        }

        public IProjection Get(int movieId, int roomId, DateTime startDate)
        {
            return db.Projections.FirstOrDefault(x => x.MovieId == movieId &&
                                                      x.RoomId == roomId &&
                                                      x.StartDate == startDate);
        }

        public IEnumerable<IProjection> GetActiveProjections(int roomId)
        {
            DateTime now = DateTime.UtcNow;

            return db.Projections.Where(x => x.RoomId == roomId &&
                                             x.StartDate > now);
        }

        public void Insert(IProjectionCreation proj)
        {
            Projection newProj = new Projection(proj.MovieId, proj.RoomId, proj.StartDate);

            db.Projections.Add(newProj);
            db.SaveChanges();
        }

        public IProjection Get(long projectionId)
        {
            return db.Projections.FirstOrDefault(x => x.Id == projectionId);
        }

        public void DecreaseAvailableSeatCount(long projID)
        {
            var entity = Get(projID);
            entity.AvailableSeatsCount--;
            db.SaveChanges();

        }

        public void IncreaseAvailableSeatCount(long projID)
        {
            var entity = Get(projID);
            entity.AvailableSeatsCount++;
            db.SaveChanges();
        }

        public string GetProjectionMovieName(long projID)
        {
            return db.Projections.FirstOrDefault(x => x.Id == projID).Movie.Name;
        }

    }
}