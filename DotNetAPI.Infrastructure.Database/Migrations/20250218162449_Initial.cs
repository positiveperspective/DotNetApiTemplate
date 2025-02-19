using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetAPI.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderCustomerID = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    RequestQty = table.Column<int>(type: "int", nullable: false),
                    ServiceDate1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceDate2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceDate3 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderBondIsPaid = table.Column<bool>(type: "bit", nullable: false),
                    OrderBondPaidDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderDifferenceIsPaid = table.Column<bool>(type: "bit", nullable: false),
                    OrderDifferenceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderPriority = table.Column<int>(type: "int", nullable: false),
                    OrderIsDispatched = table.Column<bool>(type: "bit", nullable: false),
                    OrderShippedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderProgress = table.Column<int>(type: "int", nullable: false),
                    OrderIsDelivered = table.Column<bool>(type: "bit", nullable: false),
                    OrderDeliveredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayUOrderID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayPalOrderID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoreID = table.Column<int>(type: "int", nullable: false),
                    StaffID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
