using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Person (s_Name) VALUES ('a') ");
            migrationBuilder.Sql("INSERT INTO Person (s_Name) VALUES ('b') ");
            migrationBuilder.Sql("UPDATE Person SET s_Name = 'c' WHERE [Key] = 2");

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Person",
                type: "int",
                nullable: false,
                defaultValue: 0);


            migrationBuilder.Sql("INSERT INTO Person (s_Name, Value) VALUES ('a', 3) ");
            migrationBuilder.Sql("INSERT INTO Person (s_Name, Value) VALUES ('b', 4) ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Person")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "PersonHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
