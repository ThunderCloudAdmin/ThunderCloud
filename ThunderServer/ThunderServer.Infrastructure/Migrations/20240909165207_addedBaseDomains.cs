using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThunderServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedBaseDomains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.TagId);
                    table.ForeignKey(
                        name: "FK_Tag_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThunderFolder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentFolderId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRootFolder = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThunderFolder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThunderFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FileType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    MimeType = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    EncryptedFileKey = table.Column<byte[]>(type: "bytea", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccessPermissions = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Metadata = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Checksum = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsEncrypted = table.Column<bool>(type: "boolean", nullable: false),
                    IsShared = table.Column<bool>(type: "boolean", nullable: false),
                    FileStatus = table.Column<int>(type: "integer", nullable: false),
                    ParentFolderId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThunderFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThunderFile_ThunderFolder_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "ThunderFolder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileInDirectory",
                columns: table => new
                {
                    FileInDirectoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileId = table.Column<Guid>(type: "uuid", nullable: false),
                    DirectoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileInDirectory", x => x.FileInDirectoryId);
                    table.ForeignKey(
                        name: "FK_FileInDirectory_ThunderFile_FileId",
                        column: x => x.FileId,
                        principalTable: "ThunderFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileInDirectory_ThunderFolder_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "ThunderFolder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThunderFileShare",
                columns: table => new
                {
                    FileShareId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uuid", nullable: false),
                    EncryptedFileKeyForRecipient = table.Column<byte[]>(type: "bytea", nullable: false),
                    SharedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Permission = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThunderFileShare", x => x.FileShareId);
                    table.ForeignKey(
                        name: "FK_ThunderFileShare_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThunderFileShare_AspNetUsers_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThunderFileShare_ThunderFile_FileId",
                        column: x => x.FileId,
                        principalTable: "ThunderFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThunderFileTag",
                columns: table => new
                {
                    FileTagId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThunderFileTag", x => x.FileTagId);
                    table.ForeignKey(
                        name: "FK_ThunderFileTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThunderFileTag_ThunderFile_FileId",
                        column: x => x.FileId,
                        principalTable: "ThunderFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileInDirectory_DirectoryId",
                table: "FileInDirectory",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FileInDirectory_FileId",
                table: "FileInDirectory",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_UserId",
                table: "Tag",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ThunderFile_ParentFolderId",
                table: "ThunderFile",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_ThunderFileShare_FileId",
                table: "ThunderFileShare",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ThunderFileShare_OwnerId",
                table: "ThunderFileShare",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ThunderFileShare_RecipientId",
                table: "ThunderFileShare",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_ThunderFileTag_FileId",
                table: "ThunderFileTag",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ThunderFileTag_TagId",
                table: "ThunderFileTag",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileInDirectory");

            migrationBuilder.DropTable(
                name: "ThunderFileShare");

            migrationBuilder.DropTable(
                name: "ThunderFileTag");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "ThunderFile");

            migrationBuilder.DropTable(
                name: "ThunderFolder");
        }
    }
}
