using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Middle0.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddJobIdToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "jobId",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "jobId",
                table: "Events");
        }
    }
}
