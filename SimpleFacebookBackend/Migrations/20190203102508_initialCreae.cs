using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleFacebookBackend.Migrations
{
    public partial class initialCreae : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Password = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Image = table.Column<string>(unicode: false, nullable: true),
                    Description = table.Column<string>(unicode: false, nullable: true),
                    Mail = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Phone = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    FIrst_name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Last_name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Status = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Friend",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Id_user1 = table.Column<int>(nullable: false),
                    Id_user2 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friend_User1",
                        column: x => x.Id_user1,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friend_User2",
                        column: x => x.Id_user2,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Id_sender = table.Column<int>(nullable: false),
                    Id_receiver = table.Column<int>(nullable: true),
                    Message = table.Column<string>(unicode: false, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    File = table.Column<string>(unicode: false, nullable: true),
                    Id_group = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Group",
                        column: x => x.Id_group,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_User2",
                        column: x => x.Id_receiver,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_User1",
                        column: x => x.Id_sender,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_group",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Id_group = table.Column<int>(nullable: false),
                    Id_user = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_group_group",
                        column: x => x.Id_group,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_group_user",
                        column: x => x.Id_user,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Unread",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Id_user = table.Column<int>(nullable: false),
                    Id_message = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unread", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Unread_Message",
                        column: x => x.Id_message,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Unread_User",
                        column: x => x.Id_user,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friend_Id_user1",
                table: "Friend",
                column: "Id_user1");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_Id_user2",
                table: "Friend",
                column: "Id_user2");

            migrationBuilder.CreateIndex(
                name: "IX_Message_Id_group",
                table: "Message",
                column: "Id_group");

            migrationBuilder.CreateIndex(
                name: "IX_Message_Id_receiver",
                table: "Message",
                column: "Id_receiver");

            migrationBuilder.CreateIndex(
                name: "IX_Message_Id_sender",
                table: "Message",
                column: "Id_sender");

            migrationBuilder.CreateIndex(
                name: "IX_Unread_Id_message",
                table: "Unread",
                column: "Id_message");

            migrationBuilder.CreateIndex(
                name: "IX_Unread_Id_user",
                table: "Unread",
                column: "Id_user");

            migrationBuilder.CreateIndex(
                name: "IX_User_group_Id_group",
                table: "User_group",
                column: "Id_group");

            migrationBuilder.CreateIndex(
                name: "IX_User_group_Id_user",
                table: "User_group",
                column: "Id_user");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friend");

            migrationBuilder.DropTable(
                name: "Unread");

            migrationBuilder.DropTable(
                name: "User_group");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
