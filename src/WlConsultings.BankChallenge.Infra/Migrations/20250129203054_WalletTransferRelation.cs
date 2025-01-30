using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WlConsultings.BankChallenge.Infra.Migrations
{
    /// <inheritdoc />
    public partial class WalletTransferRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Wallets_WalletId",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "WalletId",
                table: "Transfers",
                newName: "SenderWalletId");

            migrationBuilder.RenameColumn(
                name: "OriginWalletId",
                table: "Transfers",
                newName: "ReceiverWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_WalletId",
                table: "Transfers",
                newName: "IX_Transfers_SenderWalletId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Wallets",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transfers",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ReceiverWalletId",
                table: "Transfers",
                column: "ReceiverWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Wallets_ReceiverWalletId",
                table: "Transfers",
                column: "ReceiverWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Wallets_SenderWalletId",
                table: "Transfers",
                column: "SenderWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Wallets_ReceiverWalletId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Wallets_SenderWalletId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_ReceiverWalletId",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "SenderWalletId",
                table: "Transfers",
                newName: "WalletId");

            migrationBuilder.RenameColumn(
                name: "ReceiverWalletId",
                table: "Transfers",
                newName: "OriginWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_SenderWalletId",
                table: "Transfers",
                newName: "IX_Transfers_WalletId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Wallets",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transfers",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Wallets_WalletId",
                table: "Transfers",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
