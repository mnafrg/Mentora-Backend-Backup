using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentora.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CleanStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_communities_domains_DomainId1",
                table: "communities");

            migrationBuilder.DropIndex(
                name: "IX_communities_DomainId1",
                table: "communities");

            migrationBuilder.DropColumn(
                name: "DomainId1",
                table: "communities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DomainId1",
                table: "communities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_communities_DomainId1",
                table: "communities",
                column: "DomainId1");

            migrationBuilder.AddForeignKey(
                name: "FK_communities_domains_DomainId1",
                table: "communities",
                column: "DomainId1",
                principalTable: "domains",
                principalColumn: "domain_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
