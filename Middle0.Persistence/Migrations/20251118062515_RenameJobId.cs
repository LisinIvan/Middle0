using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Middle0.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameJobId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "jobId",
                table: "Events",
                newName: "JobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "Events",
                newName: "jobId");
        }
    }
}
