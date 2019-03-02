using CinemAPI.Models.Contracts.Projection;
using System;
using System.Collections.Generic;

namespace CinemAPI.Data
{
    public interface IProjectionRepository
    {
        IProjection Get(int movieId, int roomId, DateTime startDate);

        void Insert(IProjectionCreation projection);

        IEnumerable<IProjection> GetActiveProjections(int roomId);

        IProjection Get(long projectionId);

        void DecreaseAvailableSeatCount(long projID);

        void IncreaseAvailableSeatCount(long projID);

        string GetProjectionMovieName(long projID);
    }
}