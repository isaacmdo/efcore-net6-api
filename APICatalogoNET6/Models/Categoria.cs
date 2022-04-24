using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogoNET6.Models;


[Table("Categorias")]
public class Categoria  //Classe anemica (sem os cochetes do namespace)


{
    //É uma boa pratica inicializar uma coleção quando utilizamos uma coleção pela interface ICollection nas propriedades do model, fazemos isso no construtor
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }
    
    [Key] //Define a chave primeira da tabela(Nesse caso  o EF ja ia entender que essa propriedade seria a chave primaira sem a necessidade da tag, por conta da regra a baixo
    public int CategoriaId { get; set; } // Podemos usar Id ou o nome da classe + a palavra Id para EFCORE entender que essa propriedade e a chave primaria


    [Required] //Define que o campo e obrigatorio
    [StringLength(300)]//Define o tamanho da string
    public string? Nome { get; set; }

    public string? ImagemUrl { get; set; }


    public ICollection<Produto>? Produtos {  get; set; } //Aqui definimos para o EF que cada categorias tem uma colecao de produtos pela interface ICollection implementando o Tipo Produto


}
