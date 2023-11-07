using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ED2023.Database.Migrations
{
    /// <inheritdoc />
    public partial class PaymentsManyMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Payments_PaymentId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_PaymentId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Services");

            migrationBuilder.CreateTable(
                name: "PaymentService",
                columns: table => new
                {
                    PaymentsId = table.Column<int>(type: "int", nullable: false),
                    ServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentService", x => new { x.PaymentsId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_PaymentService_Payments_PaymentsId",
                        column: x => x.PaymentsId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentService_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentService_ServicesId",
                table: "PaymentService",
                column: "ServicesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentService");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_PaymentId",
                table: "Services",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Payments_PaymentId",
                table: "Services",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id");
        }
    }
}
