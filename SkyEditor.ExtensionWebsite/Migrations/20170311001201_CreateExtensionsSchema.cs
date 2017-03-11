using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SkyEditor.ExtensionWebsite.Migrations
{
    public partial class CreateExtensionsSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExtensionCollection",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ParentID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionCollection", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExtensionCollection_ExtensionCollection_ParentID",
                        column: x => x.ParentID,
                        principalTable: "ExtensionCollection",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApiKey",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CollectionID = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKey", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ApiKey_ExtensionCollection_CollectionID",
                        column: x => x.CollectionID,
                        principalTable: "ExtensionCollection",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Extension",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: false),
                    CollectionID = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ExtensionID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extension", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Extension_ExtensionCollection_CollectionID",
                        column: x => x.CollectionID,
                        principalTable: "ExtensionCollection",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtensionVersion",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    ExtensionID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionVersion", x => new { x.ID, x.Version });
                    table.ForeignKey(
                        name: "FK_ExtensionVersion_Extension_ExtensionID",
                        column: x => x.ExtensionID,
                        principalTable: "Extension",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiKey_CollectionID",
                table: "ApiKey",
                column: "CollectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Extension_CollectionID",
                table: "Extension",
                column: "CollectionID");

            migrationBuilder.CreateIndex(
                name: "IX_ExtensionCollection_ParentID",
                table: "ExtensionCollection",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_ExtensionVersion_ExtensionID",
                table: "ExtensionVersion",
                column: "ExtensionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKey");

            migrationBuilder.DropTable(
                name: "ExtensionVersion");

            migrationBuilder.DropTable(
                name: "Extension");

            migrationBuilder.DropTable(
                name: "ExtensionCollection");
        }
    }
}
