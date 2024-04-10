using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineRetailStoreV01.Migrations
{
    /// <inheritdoc />
    public partial class changed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(name: "Full Name", type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailAddress = table.Column<string>(name: "Email Address", type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HashedPassword = table.Column<string>(name: "Hashed Password", type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserType = table.Column<int>(name: "User Type", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
