using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace joro.too.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class slightDbTweak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "RatingsSum",
                table: "Shows",
                type: "real",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "RatingsSum",
                table: "Movies",
                type: "real",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RatedMovies",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RatedShows",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Director", "RatingsSum" },
                values: new object[] { null, 100f });

            migrationBuilder.UpdateData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Director", "MediaImgSrc", "RatingsSum" },
                values: new object[] { null, "https://res.cloudinary.com/djubwo5uq/image/upload/v1744470967/eycbyrohr6vtvjqamn3e.jpg", 85f });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Director",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Director",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "RatedMovies",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RatedShows",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<long>(
                name: "RatingsSum",
                table: "Shows",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<long>(
                name: "RatingsSum",
                table: "Movies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                column: "RatingsSum",
                value: 100L);

            migrationBuilder.UpdateData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "MediaImgSrc", "RatingsSum" },
                values: new object[] { "https://res.cloudinary.com/djubwo5uq/image/upload/v1742844720/lhopqfkq2ehjf7ljzlcz.jpg", 85L });
        }
    }
}
