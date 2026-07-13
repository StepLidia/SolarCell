using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarProfitabilityEstimator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackingId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TrackingId",
                table: "SolarEstimates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingId",
                table: "SolarEstimates");
        }
    }
}
