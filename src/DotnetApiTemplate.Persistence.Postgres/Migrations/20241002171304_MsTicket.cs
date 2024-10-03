using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiTemplate.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class MsTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lokasi",
                table: "MsEventBroker",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "JumlahTiket",
                table: "MsEventBroker",
                newName: "CountTicket");

            migrationBuilder.RenameColumn(
                name: "Lokasi",
                table: "MsEvent",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "JumlahTiket",
                table: "MsEvent",
                newName: "CountTicket");

            migrationBuilder.CreateTable(
                name: "MsTicket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrainName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    CountTicket = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_MsTicket", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MsTicket_CreatedByFullName",
                table: "MsTicket",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsTicket_CreatedByName",
                table: "MsTicket",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_MsTicket_Id",
                table: "MsTicket",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_MsTicket_LastUpdatedBy",
                table: "MsTicket",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MsTicket_LastUpdatedByFullName",
                table: "MsTicket",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsTicket_LastUpdatedByName",
                table: "MsTicket",
                column: "LastUpdatedByName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MsTicket");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "MsEventBroker",
                newName: "Lokasi");

            migrationBuilder.RenameColumn(
                name: "CountTicket",
                table: "MsEventBroker",
                newName: "JumlahTiket");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "MsEvent",
                newName: "Lokasi");

            migrationBuilder.RenameColumn(
                name: "CountTicket",
                table: "MsEvent",
                newName: "JumlahTiket");
        }
    }
}
