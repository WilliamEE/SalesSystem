using Microsoft.EntityFrameworkCore.Migrations;

namespace APISalesSystem.Migrations
{
    public partial class Migracionparaagregarid_usuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "id_Usuario",
                table: "SolicitudDeAfiliacion",
                unicode: false,
                maxLength: 400,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id_Usuario",
                table: "SolicitudDeAfiliacion");
        }
    }
}
