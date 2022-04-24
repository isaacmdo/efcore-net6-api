using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogoNET6.Models;



[Table("Produtos")]
public class Produto //Classe anemica
{

    [Key] //Define a chave primeira da tabela(Nesse caso  o EF ja ia entender que essa propriedade seria a chave primaira sem a necessidade da tag, por conta da regra a baixo
    public int ProdutoId { get; set; }  // Podemos usar Id ou o nome da classe(Produto) + a palavra Id = ProdutoId, para assim EFCORE entender que essa propriedade e a chave primaria

    [Required] //Define o campo como obrigatorio  EF Migrations na propriedade Nome
    [StringLength(80)] //Define o tamanho da string  no EF Migrations na propriedade Nome
    public string? Nome { get; set; }

    [Required] //Define o campo como obrigatorio no EF Migrations na propriedade Descricao
    [StringLength(300)] //Define o tamanho da string  no EF Migrations na propriedade Descricao
    public string? Descricao { get; set; }

    [Required] //Define o campo como obrigatorio no EF Migrations na propriedade Preco
    [Column(TypeName ="decimal(10,2)")] //Define o formato da coluna no EF Migrations na propriedade Preco
    public decimal Preco { get; set; }

    [Required] //Define o campo como obrigatorio no EF Migrations na propriedade ImagemUrl
    [StringLength(300)] //Define o tamanho da string no EF Migrations na propriedade ImagemUrl
    public string? ImagemUrl { get; set; }

    public float Estoque { get; set; }

    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; }  //A tabela categoria esta relacioda com produto, entao se ambas tem a mesma propriedade, sera gerada uma FK

    [JsonIgnore] //Aqui ignoramos a propriedade na derializacao e deserializacao do retorno json do model, pois esta propriedade e apenas um relacionamento para o EFCORE
    public Categoria? Categoria { get; set; }  //Aqui definimos que um produto pode conter apenas uma categoria para o EF reconhecer o relacionamento

}
