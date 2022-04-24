using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogoNET6.Migrations
{
    public partial class PopulaCategorias2 : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into categorias(Nome, ImagemUrl) Values('Bebidas', 'bebidas.jpg')");
            mb.Sql("Insert into categorias(Nome, ImagemUrl) Values('Lanches', 'lanches.jpg')");
            mb.Sql("Insert into categorias(Nome, ImagemUrl) Values('Sobremesas', 'sobremesas.jpg')");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from categorias");

        }
    }
}
