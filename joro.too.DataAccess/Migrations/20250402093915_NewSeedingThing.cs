using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace joro.too.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewSeedingThing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "vidsrc",
                table: "Movies",
                newName: "VidSrc");

            migrationBuilder.AddColumn<int>(
                name: "RatedCount",
                table: "Shows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "RatingsSum",
                table: "Shows",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "RatedCount",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "RatingsSum",
                table: "Movies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<string>(
                name: "imgsrc",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Description", "MediaImgSrc", "Name", "RatedCount", "RatingsSum", "VidSrc" },
                values: new object[] { 1, "All fairytale creatures get kicked out and get put in the swamp of an grumpy ogre, who has to follow a quest to get them out.", "https://res.cloudinary.com/djubwo5uq/image/upload/v1743439766/wtygxcolnsqykqq2ktzr.jpg", "Shrek", 13, 100L, "https://res.cloudinary.com/djubwo5uq/video/upload/v1743439842/jebwtwrywpzzlmzzlxrs.mp4" });

            migrationBuilder.InsertData(
                table: "Shows",
                columns: new[] { "Id", "Description", "MediaImgSrc", "Name", "RatedCount", "RatingsSum" },
                values: new object[] { 1, "Monkey D Luffy sets out to sea to find the legendary pirate treasure One Piece!", "https://res.cloudinary.com/djubwo5uq/image/upload/v1742844720/lhopqfkq2ehjf7ljzlcz.jpg", "One Piece", 10, 85L });

            migrationBuilder.InsertData(
                table: "GenresMovies",
                columns: new[] { "GenreId", "MovieId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 4, 1 }
                });

            migrationBuilder.InsertData(
                table: "GenresShows",
                columns: new[] { "GenreId", "ShowId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 5, 1 }
                });

            migrationBuilder.InsertData(
                table: "Seasons",
                columns: new[] { "Id", "Name", "Number", "ShowId" },
                values: new object[,]
                {
                    { 1, "Romance Dawn", 1, 1 },
                    { 2, "Orange Town", 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "Episodes",
                columns: new[] { "Id", "SeasonId", "name", "vidsrc" },
                values: new object[,]
                {
                    { 1, 1, "Im Luffy, the man who'll become King of the Pirates!", "https://res.cloudinary.com/djubwo5uq/video/upload/v1742844729/vcszzljfpaz5sktkbzh3.mp4" },
                    { 2, 1, "Pirate Hunter Zoro Appears!", "https://res.cloudinary.com/djubwo5uq/video/upload/v1742844740/gijx1zxfotjdp2jkyqjb.mp4" },
                    { 3, 1, "The Monstrous Captain Morgan", "https://res.cloudinary.com/djubwo5uq/video/upload/v1742844754/qp8rzyzmn1twghkidywx.mp4" },
                    { 4, 1, "Luffy's past. The Red-Haired Shanks.", "https://res.cloudinary.com/djubwo5uq/video/upload/v1742844768/jsw1lmzcngmelb9tdir6.mp4" },
                    { 5, 2, "Fear! The Mysterious Clown Pirate Captain Buggy!", "https://res.cloudinary.com/djubwo5uq/video/upload/v1743544387/oobotrj7rjyrrluueovs.mp4" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "GenresMovies",
                keyColumns: new[] { "GenreId", "MovieId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "GenresMovies",
                keyColumns: new[] { "GenreId", "MovieId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "GenresMovies",
                keyColumns: new[] { "GenreId", "MovieId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "GenresShows",
                keyColumns: new[] { "GenreId", "ShowId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "GenresShows",
                keyColumns: new[] { "GenreId", "ShowId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "GenresShows",
                keyColumns: new[] { "GenreId", "ShowId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Seasons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Seasons",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "RatedCount",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "RatingsSum",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "RatedCount",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "RatingsSum",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "imgsrc",
                table: "Actors");

            migrationBuilder.RenameColumn(
                name: "VidSrc",
                table: "Movies",
                newName: "vidsrc");

            migrationBuilder.AddColumn<string>(
                name: "Rating",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rating",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
