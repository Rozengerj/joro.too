using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace joro.too.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class plssssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RatedShows",
                table: "AspNetUsers",
                newName: "RatedShowsIds");

            migrationBuilder.RenameColumn(
                name: "RatedMovies",
                table: "AspNetUsers",
                newName: "RatedMovieIds");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Shows",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Movies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Shows_UserId",
                table: "Shows",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_UserId",
                table: "Movies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_UserId",
                table: "Movies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_AspNetUsers_UserId",
                table: "Shows",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_UserId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Shows_AspNetUsers_UserId",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Shows_UserId",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Movies_UserId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "RatedShowsIds",
                table: "AspNetUsers",
                newName: "RatedShows");

            migrationBuilder.RenameColumn(
                name: "RatedMovieIds",
                table: "AspNetUsers",
                newName: "RatedMovies");
        }
    }
}
