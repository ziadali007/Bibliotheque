using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cart_biblio.Migrations
{
    /// <inheritdoc />
    public partial class yarab1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    nationalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    fullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isAdmin = table.Column<bool>(type: "bit", nullable: false),
                    telephone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    homeAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    confirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.nationalId);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    book_name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    borrowable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    book_pic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.book_name);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    national_id = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    client_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telephone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    email_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    home_address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.national_id);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    operation_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    national_id = table.Column<string>(type: "nvarchar(14)", nullable: false),
                    book_name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.operation_id);
                    table.ForeignKey(
                        name: "FK_Operations_Books_book_name",
                        column: x => x.book_name,
                        principalTable: "Books",
                        principalColumn: "book_name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operations_Clients_national_id",
                        column: x => x.national_id,
                        principalTable: "Clients",
                        principalColumn: "national_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_book_name",
                table: "Operations",
                column: "book_name");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_national_id",
                table: "Operations",
                column: "national_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
