using APICatalogoNET6.Context;
using APICatalogoNET6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogoNET6.Controllers
{
    [Route("[controller]")] ///Declaracao da rota "pai" do controller, como "controller", que é uma palavra chave do aspnetcore deixar o nome da classe "Produtos" como rota de chamada a este controller
    [ApiController] ///Define a classe como um controller da api
    public class ProdutosController : ControllerBase ///Implementa os metodos controladores da api
    {

        
        private readonly AppDbContext _context; ///Declara o AppDbContext do EF para realizacao das consultas sem a necessidade de sql explicito

        public ProdutosController(AppDbContext context)
        {
            _context = context; ///Inicia o AppDbContext do EF para realizacao das consultas sem a necessidade de sql explicito
        }

        [HttpGet] ///Define o verbo http do metodo Get()
        public ActionResult<IEnumerable<Produto>> Get() ///Retorna uma ActionResult("Usamos quando queremos retornar um status code por exemplo"), e tambem um IEnumerable, que é uma interface de somente leitura
        {
            try
            {
                var produtos = _context.Produtos.ToList(); ///Lista todos os produtos da tabela produtos pelo EFCORE com o metodo ToList();
                if (produtos is null)
                {
                    return NotFound("Produtos não encontrados"); //Retorna um status code 404 do tipo ActionResult
                }
                return produtos; ///Retorna a lista de produtos da interface IEnumerable
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação");
            }

        }


        [HttpGet("{id:int}", Name="ObterProduto")] ///Define o verbo http com um parametro id expecificado por dois pontos ":" 
        public ActionResult<Produto> Get(int id) ///Retorna um ActionResult e recebe um id pelo parametro da rota "/Produtos/2" por exemplo, o id recebido seria "2"
        {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id); /// Manipula o parametro recebido pelo EFCORE
                if (produto is null)
                {
                    return NotFound("Produto não encontrado");///Retorna um status code 404 do tipo ActionResult
                }
                return produto; ///Retorna um produto
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação");
            }

        }

        [HttpPost] //Define o verbo http de request
        public ActionResult Post(Produto produto) //retorna um status code pelo ActionResult
        {
            try
            {
                if (produto is null) return BadRequest(); //caso o posto receber o parametro como nulo, retorna um 404

                _context.Produtos.Add(produto); //Utiliza da classe de contexto do EFCORE para adicionar um produto na tabela com o metodo Add
                _context.SaveChanges(); //Salva o estado do contexto no banco
                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto); //Aqui retornamos um status code 204 e no output realizamos uma chamada de retorna que esta com o name de ObterProduto no caso nosso Get que recebo o parametro Id, assim conseguimos mostrar para o usuario como foi criado o produto no banco
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação");
            }

        }

        [HttpPut("{id:int}")]  //Define o verbo http 
        public ActionResult Put(int id, Produto produto) //Retorna uma status code 
        {
            try
            {
                if (id != produto.ProdutoId) //Caso o parametro id for diferente do id objeto parametro produto que sera editado, retorna um 404
                {
                    return BadRequest();//Retorna status 404
                }

                _context.Entry(produto).State = EntityState.Modified; //Modifica o estado do produto
                _context.SaveChanges(); //Salva o novo estado no banco

                return Ok(produto); //Retorna status 200 e o corpo do produto
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação");
            }

        }

        [HttpDelete("{id:int}")] //Define o verbo http
        public ActionResult Delete(int id) //retorna um status code
        {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id); //Retorna o produto ou null se nao encontrar

                if (produto is null)
                {
                    return NotFound("Produto não localizado"); //Retorna um 404
                }

                _context.Produtos.Remove(produto); //Remove o estado do context do produto
                _context.SaveChanges(); //Executa a remocao

                return Ok(produto); //Retorna status code 200 e produto como corpo
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação");
            }

        }
    }


}
