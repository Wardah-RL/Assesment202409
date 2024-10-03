using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiTemplate.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class updateEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "MsEventBroker");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "MsEvent");

            migrationBuilder.CreateTable(
                name: "MsEventLocation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_MsEventLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MsEventLocation_MsEvent_EventId",
                        column: x => x.EventId,
                        principalTable: "MsEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MsEventLocationBroker",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    EventBrokerId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_MsEventLocationBroker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MsEventLocationBroker_MsEventBroker_EventBrokerId",
                        column: x => x.EventBrokerId,
                        principalTable: "MsEventBroker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocation_CreatedByFullName",
                table: "MsEventLocation",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocation_CreatedByName",
                table: "MsEventLocation",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocation_EventId",
                table: "MsEventLocation",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocation_Id",
                table: "MsEventLocation",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocation_LastUpdatedBy",
                table: "MsEventLocation",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocation_LastUpdatedByFullName",
                table: "MsEventLocation",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocation_LastUpdatedByName",
                table: "MsEventLocation",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocationBroker_CreatedByFullName",
                table: "MsEventLocationBroker",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocationBroker_CreatedByName",
                table: "MsEventLocationBroker",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocationBroker_EventBrokerId",
                table: "MsEventLocationBroker",
                column: "EventBrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocationBroker_Id",
                table: "MsEventLocationBroker",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocationBroker_LastUpdatedBy",
                table: "MsEventLocationBroker",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocationBroker_LastUpdatedByFullName",
                table: "MsEventLocationBroker",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventLocationBroker_LastUpdatedByName",
                table: "MsEventLocationBroker",
                column: "LastUpdatedByName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MsEventLocation");

            migrationBuilder.DropTable(
                name: "MsEventLocationBroker");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "MsEventBroker",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "MsEvent",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");
        }
    }
}
