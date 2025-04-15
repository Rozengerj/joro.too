using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace joro.too.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class actorRolesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "ActorsRolesShows",
                newName: "Roles");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "ActorsRolesMovies",
                newName: "Roles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Roles",
                table: "ActorsRolesShows",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "Roles",
                table: "ActorsRolesMovies",
                newName: "Role");
        }
    }
}
