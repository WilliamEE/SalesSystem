using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APISalesSystem.Migrations
{
    public partial class Migracioninicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categoria",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    id_padre = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoria", x => x.id);
                    table.ForeignKey(
                        name: "FK_categoria_categoria",
                        column: x => x.id_padre,
                        principalTable: "categoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudDeAfiliacion",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha = table.Column<DateTime>(type: "date", nullable: true),
                    referenciaBancariaUrl = table.Column<string>(unicode: false, maxLength: 400, nullable: true),
                    reciboLuzUrl = table.Column<string>(unicode: false, maxLength: 400, nullable: true),
                    ReciboAguaUrl = table.Column<string>(unicode: false, maxLength: 400, nullable: true),
                    reciboTelefonoUrl = table.Column<string>(unicode: false, maxLength: 400, nullable: true),
                    pagareUrl = table.Column<string>(unicode: false, maxLength: 400, nullable: true),
                    estado = table.Column<string>(unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudDeAfiliacion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "producto",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    precio = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    imagenUrl = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    id_categoria = table.Column<int>(nullable: true),
                    id_perfil = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producto", x => x.id);
                    table.ForeignKey(
                        name: "FK_producto_categoria",
                        column: x => x.id_categoria,
                        principalTable: "categoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    correo = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    contraseña = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    id_rol = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id);
                    table.ForeignKey(
                        name: "FK_usuario_rol",
                        column: x => x.id_rol,
                        principalTable: "rol",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "perfilDeUsuario",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    fotoDePerfil = table.Column<string>(unicode: false, maxLength: 400, nullable: true),
                    id_usuario = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_perfilDeUsuario_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categoria_id_padre",
                table: "categoria",
                column: "id_padre");

            migrationBuilder.CreateIndex(
                name: "IX_perfilDeUsuario_id_usuario",
                table: "perfilDeUsuario",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_producto_id_categoria",
                table: "producto",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_id_rol",
                table: "usuario",
                column: "id_rol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "perfilDeUsuario");

            migrationBuilder.DropTable(
                name: "producto");

            migrationBuilder.DropTable(
                name: "SolicitudDeAfiliacion");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "categoria");

            migrationBuilder.DropTable(
                name: "rol");
        }
    }
}
