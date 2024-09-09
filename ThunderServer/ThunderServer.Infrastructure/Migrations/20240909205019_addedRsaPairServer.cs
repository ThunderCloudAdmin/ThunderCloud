using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThunderServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedRsaPairServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RsaKeyPairServerId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "RsaKeyPairServer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Salt = table.Column<byte[]>(type: "bytea", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PublicKey = table.Column<byte[]>(type: "bytea", nullable: false),
                    PrivateKey = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RsaKeyPairServer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RsaKeyPairServer_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RsaKeyPairServer_UserId",
                table: "RsaKeyPairServer",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RsaKeyPairServer");

            migrationBuilder.DropColumn(
                name: "RsaKeyPairServerId",
                table: "AspNetUsers");
        }
    }
}
