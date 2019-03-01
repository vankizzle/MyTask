using CinemAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemAPI.Data.EF.ModelConfigurations
{
    internal sealed class ReservationTicketModelConfiguration : IModelConfiguration
    {
        public void Configure(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<ReservationTicket> roomModel = modelBuilder.Entity<ReservationTicket>();
            roomModel.HasKey(model => model.ID);
            roomModel.Property(model => model.ProjectionStartDate).IsRequired();
            roomModel.Property(model => model.MovieName).IsRequired();
            roomModel.Property(model => model.CinemaName).IsRequired();
            roomModel.Property(model => model.RoomNumber).IsRequired();
            roomModel.Property(model => model.Row).IsRequired();
            roomModel.Property(model => model.Column).IsRequired();
        }
    }
}
