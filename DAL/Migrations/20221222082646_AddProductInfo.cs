using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddProductInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "s_Info",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "[s_Name] + ' ' + Str([Price]) + 'zł'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "s_Info",
                table: "Products");
        }
    }
}
