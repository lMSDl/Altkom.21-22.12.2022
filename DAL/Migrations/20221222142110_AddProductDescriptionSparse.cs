using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddProductDescriptionSparse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "s_Description",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true)
                .Annotation("SqlServer:Sparse", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "s_Description",
                table: "Product");
        }
    }
}
