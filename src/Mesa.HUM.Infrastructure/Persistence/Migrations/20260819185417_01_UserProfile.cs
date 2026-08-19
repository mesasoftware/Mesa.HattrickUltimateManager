using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mesa.HUM.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class _01_UserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", fixedLength: true, maxLength: 36, nullable: false),
                    chpp_token_token = table.Column<string>(type: "TEXT", fixedLength: true, maxLength: 36, nullable: true),
                    chpp_token_token_secret = table.Column<string>(type: "TEXT", fixedLength: true, maxLength: 36, nullable: true),
                    chpp_token_obtained_at = table.Column<DateTimeOffset>(type: "TEXT", fixedLength: true, maxLength: 23, nullable: true),
                    chpp_token_expires_at = table.Column<DateTimeOffset>(type: "TEXT", fixedLength: true, maxLength: 23, nullable: true),
                    synchronized_at = table.Column<DateTimeOffset>(type: "TEXT", fixedLength: true, maxLength: 23, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_profiles", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_profiles_id",
                table: "user_profiles",
                column: "id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_profiles");
        }
    }
}
