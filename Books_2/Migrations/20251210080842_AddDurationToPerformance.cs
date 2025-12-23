using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Books_2.Migrations
{
    /// <inheritdoc />
    public partial class AddDurationToFilmScreening : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "FilmScreenings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "FilmScreenings");
        }
    }
}
