using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixProductCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM [Categories] WHERE [Id] = 1
                )
                BEGIN
                    SET IDENTITY_INSERT [Categories] ON;

                    INSERT INTO [Categories] ([Id], [Name], [DisplayOrder])
                    VALUES (1, 'Action', 1);

                    SET IDENTITY_INSERT [Categories] OFF;
                END;

                IF NOT EXISTS (
                    SELECT 1 FROM [Categories] WHERE [Id] = 2
                )
                BEGIN
                    SET IDENTITY_INSERT [Categories] ON;

                    INSERT INTO [Categories] ([Id], [Name], [DisplayOrder])
                    VALUES (2, 'SciFi', 2);

                    SET IDENTITY_INSERT [Categories] OFF;
                END;

                IF NOT EXISTS (
                    SELECT 1 FROM [Categories] WHERE [Id] = 3
                )
                BEGIN
                    SET IDENTITY_INSERT [Categories] ON;

                    INSERT INTO [Categories] ([Id], [Name], [DisplayOrder])
                    VALUES (3, 'History', 3);

                    SET IDENTITY_INSERT [Categories] OFF;
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
