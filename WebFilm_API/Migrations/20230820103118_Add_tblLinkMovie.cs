using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebFilm_API.Migrations
{
    /// <inheritdoc />
    public partial class Add_tblLinkMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LinkServerId",
                table: "Episodes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LinkServers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkServers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_LinkServerId",
                table: "Episodes",
                column: "LinkServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_LinkServers_LinkServerId",
                table: "Episodes",
                column: "LinkServerId",
                principalTable: "LinkServers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_LinkServers_LinkServerId",
                table: "Episodes");

            migrationBuilder.DropTable(
                name: "LinkServers");

            migrationBuilder.DropIndex(
                name: "IX_Episodes_LinkServerId",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "LinkServerId",
                table: "Episodes");
        }
    }
}
