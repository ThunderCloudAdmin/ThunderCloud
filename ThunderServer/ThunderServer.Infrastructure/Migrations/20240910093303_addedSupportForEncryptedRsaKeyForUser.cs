using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThunderServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedSupportForEncryptedRsaKeyForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrivateKey",
                table: "RsaKeyPairServer");

            migrationBuilder.DropColumn(
                name: "PublicKey",
                table: "RsaKeyPairServer");

            migrationBuilder.RenameColumn(
                name: "Salt",
                table: "RsaKeyPairServer",
                newName: "EncryptedPrivateRsaKey");

            migrationBuilder.CreateTable(
                name: "Argon2Key",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Salt = table.Column<byte[]>(type: "bytea", nullable: false),
                    Iterations = table.Column<int>(type: "integer", nullable: false),
                    MemorySize = table.Column<int>(type: "integer", nullable: false),
                    Parallelism = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Argon2Key", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RsaKeyPairServer_Argon2Key_Id",
                table: "RsaKeyPairServer",
                column: "Id",
                principalTable: "Argon2Key",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RsaKeyPairServer_Argon2Key_Id",
                table: "RsaKeyPairServer");

            migrationBuilder.DropTable(
                name: "Argon2Key");

            migrationBuilder.RenameColumn(
                name: "EncryptedPrivateRsaKey",
                table: "RsaKeyPairServer",
                newName: "Salt");

            migrationBuilder.AddColumn<byte[]>(
                name: "PrivateKey",
                table: "RsaKeyPairServer",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PublicKey",
                table: "RsaKeyPairServer",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
