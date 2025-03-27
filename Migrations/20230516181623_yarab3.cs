using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cart_biblio.Migrations
{
    /// <inheritdoc />
    public partial class yarab3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bridges",
                columns: table => new
                {
                    book_name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nationalId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bridges", x => new { x.book_name, x.nationalId });
                    table.ForeignKey(
                        name: "FK_Bridges_Accounts_nationalId",
                        column: x => x.nationalId,
                        principalTable: "Accounts",
                        principalColumn: "nationalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bridges_Books_book_name",
                        column: x => x.book_name,
                        principalTable: "Books",
                        principalColumn: "book_name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    cartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    book_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    book_name1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RegisterModelnationalId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.cartId);
                    table.ForeignKey(
                        name: "FK_Carts_Accounts_RegisterModelnationalId",
                        column: x => x.RegisterModelnationalId,
                        principalTable: "Accounts",
                        principalColumn: "nationalId");
                    table.ForeignKey(
                        name: "FK_Carts_Books_book_name1",
                        column: x => x.book_name1,
                        principalTable: "Books",
                        principalColumn: "book_name");
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    cartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    book_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userNationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CartscartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.cartItemId);
                    table.ForeignKey(
                        name: "FK_CartItem_Carts_CartscartId",
                        column: x => x.CartscartId,
                        principalTable: "Carts",
                        principalColumn: "cartId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bridges_nationalId",
                table: "Bridges",
                column: "nationalId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartscartId",
                table: "CartItem",
                column: "CartscartId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_book_name1",
                table: "Carts",
                column: "book_name1");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_RegisterModelnationalId",
                table: "Carts",
                column: "RegisterModelnationalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bridges");

            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "Carts");
        }
    }
}
