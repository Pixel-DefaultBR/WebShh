using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShell.Migrations
{
    /// <inheritdoc />
    public partial class Change_Table_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Command",
                table: "Command");

            migrationBuilder.RenameTable(
                name: "Command",
                newName: "Commands");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Commands",
                table: "Commands",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Commands",
                table: "Commands");

            migrationBuilder.RenameTable(
                name: "Commands",
                newName: "Command");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Command",
                table: "Command",
                column: "id");
        }
    }
}
