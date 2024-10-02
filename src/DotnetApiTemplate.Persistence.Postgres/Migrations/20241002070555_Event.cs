using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiTemplate.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Event : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MsEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Lokasi = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    JumlahTiket = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MsEventBroker",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Lokasi = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    JumlahTiket = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsEventBroker", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_CreatedByFullName",
                table: "MsEvent",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_CreatedByName",
                table: "MsEvent",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_Id",
                table: "MsEvent",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_LastUpdatedBy",
                table: "MsEvent",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_LastUpdatedByFullName",
                table: "MsEvent",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_LastUpdatedByName",
                table: "MsEvent",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventBroker_CreatedByFullName",
                table: "MsEventBroker",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventBroker_CreatedByName",
                table: "MsEventBroker",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventBroker_Id",
                table: "MsEventBroker",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventBroker_LastUpdatedBy",
                table: "MsEventBroker",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventBroker_LastUpdatedByFullName",
                table: "MsEventBroker",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventBroker_LastUpdatedByName",
                table: "MsEventBroker",
                column: "LastUpdatedByName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MsEvent");

            migrationBuilder.DropTable(
                name: "MsEventBroker");
        }
    }
}
