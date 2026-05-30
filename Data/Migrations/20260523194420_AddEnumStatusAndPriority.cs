using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DayFlowAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEnumStatusAndPriority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Adicionar colunas temporárias com tipo integer (nullable)
            migrationBuilder.AddColumn<int>(
                name: "Status_temp",
                table: "ToDoItems",
                type: "integer",
                nullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "Priority_temp",
                table: "ToDoItems",
                type: "integer",
                nullable: true
            );

            // 2. Preencher as colunas temporárias com o valor inteiro correspondente à string original
            migrationBuilder.Sql(
                @"UPDATE ""ToDoItems""
          SET ""Status_temp"" = CASE ""Status""
              WHEN 'Pending' THEN 1
              WHEN 'InProgress' THEN 2
              WHEN 'Completed' THEN 3
              ELSE NULL
          END,
          ""Priority_temp"" = CASE ""Priority""
              WHEN 'Low' THEN 1
              WHEN 'Medium' THEN 2
              WHEN 'High' THEN 3
              ELSE NULL
          END;"
            );

            // 3. Remover as colunas originais
            migrationBuilder.DropColumn(name: "Status", table: "ToDoItems");

            migrationBuilder.DropColumn(name: "Priority", table: "ToDoItems");

            // 4. Renomear as colunas temporárias para os nomes definitivos
            migrationBuilder.RenameColumn(
                name: "Status_temp",
                table: "ToDoItems",
                newName: "Status"
            );

            migrationBuilder.RenameColumn(
                name: "Priority_temp",
                table: "ToDoItems",
                newName: "Priority"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reversão: volta a ser string, com mapeamento inverso
            migrationBuilder.AddColumn<string>(
                name: "Status_temp",
                table: "ToDoItems",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "Priority_temp",
                table: "ToDoItems",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true
            );

            migrationBuilder.Sql(
                @"UPDATE ""ToDoItems""
          SET ""Status_temp"" = CASE ""Status""
              WHEN 1 THEN 'Pending'
              WHEN 2 THEN 'InProgress'
              WHEN 3 THEN 'Completed'
              ELSE NULL
          END,
          ""Priority_temp"" = CASE ""Priority""
              WHEN 1 THEN 'Low'
              WHEN 2 THEN 'Medium'
              WHEN 3 THEN 'High'
              ELSE NULL
          END;"
            );

            migrationBuilder.DropColumn(name: "Status", table: "ToDoItems");

            migrationBuilder.DropColumn(name: "Priority", table: "ToDoItems");

            migrationBuilder.RenameColumn(
                name: "Status_temp",
                table: "ToDoItems",
                newName: "Status"
            );

            migrationBuilder.RenameColumn(
                name: "Priority_temp",
                table: "ToDoItems",
                newName: "Priority"
            );
        }
    }
}
