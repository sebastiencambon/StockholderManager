using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace StockholderManager.Migrations
{
    public partial class add_relation_stock_holder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_AspNetUsers_HolderId",
                table: "Stocks");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_AspNetUsers_HolderId",
                table: "Stocks",
                column: "HolderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_AspNetUsers_HolderId",
                table: "Stocks");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_AspNetUsers_HolderId",
                table: "Stocks",
                column: "HolderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
