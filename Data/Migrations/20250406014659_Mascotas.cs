using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProyectoFinalAp1.Migrations
{
    /// <inheritdoc />
    public partial class Mascotas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Donador",
                columns: table => new
                {
                    DonadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donador", x => x.DonadorId);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    EmpleadoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaDeContratacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salario = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.EmpleadoId);
                });

            migrationBuilder.CreateTable(
                name: "ProductoCategorias",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IconCSS = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoCategorias", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    ProveedorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.ProveedorId);
                });

            migrationBuilder.CreateTable(
                name: "Mascotas",
                columns: table => new
                {
                    MascotaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Raza = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaDeNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Precio = table.Column<double>(type: "float", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonadorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mascotas", x => x.MascotaId);
                    table.ForeignKey(
                        name: "FK_Mascotas_Donador_DonadorId",
                        column: x => x.DonadorId,
                        principalTable: "Donador",
                        principalColumn: "DonadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    ProductoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ImagenURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<double>(type: "float", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    TipoCategoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    ProductoCategoriasCategoriaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.ProductoId);
                    table.ForeignKey(
                        name: "FK_Productos_ProductoCategorias_ProductoCategoriasCategoriaId",
                        column: x => x.ProductoCategoriasCategoriaId,
                        principalTable: "ProductoCategorias",
                        principalColumn: "CategoriaId");
                    table.ForeignKey(
                        name: "FK_Productos_Proveedores_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedores",
                        principalColumn: "ProveedorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CarritoMascotas",
                columns: table => new
                {
                    CarritoMascotaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreMascota = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MascotaId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    MascotasMascotaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarritoMascotas", x => x.CarritoMascotaId);
                    table.ForeignKey(
                        name: "FK_CarritoMascotas_Mascotas_MascotaId",
                        column: x => x.MascotaId,
                        principalTable: "Mascotas",
                        principalColumn: "MascotaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarritoMascotas_Mascotas_MascotasMascotaId",
                        column: x => x.MascotasMascotaId,
                        principalTable: "Mascotas",
                        principalColumn: "MascotaId");
                });

            migrationBuilder.CreateTable(
                name: "Citas",
                columns: table => new
                {
                    CitaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreMascota = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false),
                    MascotasMascotaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas", x => x.CitaId);
                    table.ForeignKey(
                        name: "FK_Citas_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "EmpleadoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citas_Mascotas_MascotasMascotaId",
                        column: x => x.MascotasMascotaId,
                        principalTable: "Mascotas",
                        principalColumn: "MascotaId");
                });

            migrationBuilder.CreateTable(
                name: "Carrito",
                columns: table => new
                {
                    CarritoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carrito", x => x.CarritoId);
                    table.ForeignKey(
                        name: "FK_Carrito_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "ProductoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FacturaMascotas",
                columns: table => new
                {
                    FacturaMascotaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MascotaId = table.Column<int>(type: "int", nullable: false),
                    CarritoMascotaId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturaMascotas", x => x.FacturaMascotaId);
                    table.ForeignKey(
                        name: "FK_FacturaMascotas_CarritoMascotas_CarritoMascotaId",
                        column: x => x.CarritoMascotaId,
                        principalTable: "CarritoMascotas",
                        principalColumn: "CarritoMascotaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacturaMascotas_Mascotas_MascotaId",
                        column: x => x.MascotaId,
                        principalTable: "Mascotas",
                        principalColumn: "MascotaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    FacturaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    CarritoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.FacturaId);
                    table.ForeignKey(
                        name: "FK_Facturas_Carrito_CarritoId",
                        column: x => x.CarritoId,
                        principalTable: "Carrito",
                        principalColumn: "CarritoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facturas_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "ProductoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Donador",
                columns: new[] { "DonadorId", "Direccion", "Email", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, "N/A", "N/A", "Nadie", "0" },
                    { 2, "Calle 1, Santo Domingo", "juanperez@example.com", "Juan Pérez", "8095551111" },
                    { 3, "Av. Bolívar, Santiago", "mariagomez@example.com", "María Gómez", "8095552222" },
                    { 4, "Calle Duarte, La Vega", "carloslopez@example.com", "Carlos López", "8095553333" },
                    { 5, "Calle las Palmas, San Cristóbal", "anarodriguez@example.com", "Ana Rodríguez", "8295554444" },
                    { 6, "Av. Independencia, Puerto Plata", "pedrosanchez@example.com", "Pedro Sánchez", "8295555555" },
                    { 7, "Calle Central, Barahona", "luisfernandez@example.com", "Luis Fernández", "8495556666" },
                    { 8, "Calle Norte, Higuey", "gloriamartinez@example.com", "Gloria Martínez", "8495557777" },
                    { 9, "Calle Este, Moca", "josedominguez@example.com", "José Domínguez", "8095558888" },
                    { 10, "Av. Sur, San Pedro", "elenavargas@example.com", "Elena Vargas", "8295559999" },
                    { 11, "Calle Oeste, Bonao", "ricardojimenez@example.com", "Ricardo Jiménez", "8095550000" },
                    { 12, "Av. Metropolitana, La Romana", "sofiacastro@example.com", "Sofía Castro", "8295561111" },
                    { 13, "Calle del Sol, Monte Plata", "andresherrera@example.com", "Andrés Herrera", "8495562222" },
                    { 14, "Calle Primavera, Baní", "carmenortiz@example.com", "Carmen Ortiz", "8095563333" },
                    { 15, "Av. Central, Nagua", "franciscomedina@example.com", "Francisco Medina", "8295564444" },
                    { 16, "Calle del Mar, Samaná", "dianaperez@example.com", "Diana Pérez", "8495565555" },
                    { 17, "Calle Principal, Azua", "albertorios@example.com", "Alberto Ríos", "8095566666" },
                    { 18, "Av. Florida, Cotuí", "fernandacastro@example.com", "Fernanda Castro", "8295567777" },
                    { 19, "Calle Sol Naciente, Jarabacoa", "estebanvargas@example.com", "Esteban Vargas", "8495568888" },
                    { 20, "Av. Primavera, Hato Mayor", "patriciaherrera@example.com", "Patricia Herrera", "8095569999" }
                });

            migrationBuilder.InsertData(
                table: "Proveedores",
                columns: new[] { "ProveedorId", "Direccion", "Email", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, "Av. 27 de Febrero #125, Santo Domingo", "contacto@felinosrd.com", "Alimentos Felinos RD", "8095551234" },
                    { 2, "Calle Duarte #45, Santiago", "ventas@nutrican.com.do", "NutriCan Dominicana", "8295555678" },
                    { 3, "Calle Las Flores #12, La Vega", "info@mascoterosrd.com", "Juguetes Mascoteros", "8495559012" },
                    { 4, "Av. Independencia #78, San Cristóbal", "pedidos@accesoriospet.com", "Accesorios Pet", "8095553456" },
                    { 5, "Calle Salud #33, Puerto Plata", "farmacia@vetrd.com", "Farmacia Veterinaria RD", "8295557890" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carrito_ProductoId",
                table: "Carrito",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_CarritoMascotas_MascotaId",
                table: "CarritoMascotas",
                column: "MascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_CarritoMascotas_MascotasMascotaId",
                table: "CarritoMascotas",
                column: "MascotasMascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_EmpleadoId",
                table: "Citas",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_MascotasMascotaId",
                table: "Citas",
                column: "MascotasMascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_FacturaMascotas_CarritoMascotaId",
                table: "FacturaMascotas",
                column: "CarritoMascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_FacturaMascotas_MascotaId",
                table: "FacturaMascotas",
                column: "MascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_CarritoId",
                table: "Facturas",
                column: "CarritoId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_ProductoId",
                table: "Facturas",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Mascotas_DonadorId",
                table: "Mascotas",
                column: "DonadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Nombre",
                table: "Productos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_ProductoCategoriasCategoriaId",
                table: "Productos",
                column: "ProductoCategoriasCategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_ProveedorId",
                table: "Productos",
                column: "ProveedorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citas");

            migrationBuilder.DropTable(
                name: "FacturaMascotas");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "CarritoMascotas");

            migrationBuilder.DropTable(
                name: "Carrito");

            migrationBuilder.DropTable(
                name: "Mascotas");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Donador");

            migrationBuilder.DropTable(
                name: "ProductoCategorias");

            migrationBuilder.DropTable(
                name: "Proveedores");
        }
    }
}
