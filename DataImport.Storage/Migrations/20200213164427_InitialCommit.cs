using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataImport.Storage.Migrations
{
    public partial class InitialCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecordsImportedData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Record = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordsImportedData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldsImportedData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RecordImportedDataId = table.Column<Guid>(nullable: false),
                    FieldName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldsImportedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldsImportedData_RecordsImportedData_RecordImportedDataId",
                        column: x => x.RecordImportedDataId,
                        principalTable: "RecordsImportedData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldsImportedData_RecordImportedDataId",
                table: "FieldsImportedData",
                column: "RecordImportedDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldsImportedData");

            migrationBuilder.DropTable(
                name: "RecordsImportedData");
        }
    }
}
