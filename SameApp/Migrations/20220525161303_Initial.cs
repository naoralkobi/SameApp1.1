using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SameApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserNameOwner = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Server = table.Column<string>(type: "TEXT", nullable: true),
                    Last = table.Column<string>(type: "TEXT", nullable: true),
                    LastDate = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => new { x.Id, x.UserNameOwner });
                    table.ForeignKey(
                        name: "FK_Contact_User_UserName",
                        column: x => x.UserName,
                        principalTable: "User",
                        principalColumn: "UserName");
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<string>(type: "TEXT", nullable: true),
                    Sent = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    ContactId = table.Column<string>(type: "TEXT", nullable: true),
                    ContactId1 = table.Column<string>(type: "TEXT", nullable: true),
                    ContactUserNameOwner = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Contact_ContactId1_ContactUserNameOwner",
                        columns: x => new { x.ContactId1, x.ContactUserNameOwner },
                        principalTable: "Contact",
                        principalColumns: new[] { "Id", "UserNameOwner" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contact_UserName",
                table: "Contact",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ContactId1_ContactUserNameOwner",
                table: "Message",
                columns: new[] { "ContactId1", "ContactUserNameOwner" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
