using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AlterCascadeProductProductCat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductCats_ProductCats_ProductCatId",
                table: "ProductProductCats");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductCats_Products_ProductId",
                table: "ProductProductCats");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductCats_ProductCats_ProductCatId",
                table: "ProductProductCats",
                column: "ProductCatId",
                principalTable: "ProductCats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductCats_Products_ProductId",
                table: "ProductProductCats",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductCats_ProductCats_ProductCatId",
                table: "ProductProductCats");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductCats_Products_ProductId",
                table: "ProductProductCats");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductCats_ProductCats_ProductCatId",
                table: "ProductProductCats",
                column: "ProductCatId",
                principalTable: "ProductCats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductCats_Products_ProductId",
                table: "ProductProductCats",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
