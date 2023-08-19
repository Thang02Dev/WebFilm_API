using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebFilm_API.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Movie_add_position : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Movies",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Movies");
        }
    }
}
