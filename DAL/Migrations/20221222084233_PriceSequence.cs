using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class PriceSequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sequences");

            migrationBuilder.CreateSequence<int>(
                name: "ProductPrice",
                schema: "sequences",
                startValue: 100L,
                incrementBy: 33,
                minValue: 10L,
                maxValue: 300L,
                cyclic: true);

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR sequences.ProductPrice",
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "ProductPrice",
                schema: "sequences");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Products",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValueSql: "NEXT VALUE FOR sequences.ProductPrice");
        }
    }
}
