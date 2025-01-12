using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunityBackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateUsersMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE TABLE Productss (
                Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                Name VARCHAR(255) NOT NULL,
                Price DECIMAL(18, 2) NOT NULL
            );
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE Productss;");
        }
    }
}
