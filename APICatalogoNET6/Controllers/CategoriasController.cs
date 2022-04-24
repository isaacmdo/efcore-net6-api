using APICatalogoNET6.Context;
using APICatalogoNET6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogoNET6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context; ///Declara o AppDbContext do EF para realizacao das consultas sem a necessidade de sql explicito

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }


        //PERFORMANCE EM CONSULTAS
        //O metodo AsNoTracking requira o rastreamento dos dados no cache, e adiciona uma performance consideravel nas consultas
        //Não retornar sempre todos os itens de uma consulta, é uma boa pratica utilizar o metodo Take(n), para limitar uma certa quantidade de item no retorno
        //Sempre que possivel definir uma condicao na consulta com o metodo Where ".Where(c => c.CategoriaId <= 5).ToList()"


        [HttpGet("produtos")] ///Define o verbo http do metodo Get()
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos() ///Retorna uma ActionResult("Usamos quando queremos retornar um status code por exemplo"), e tambem um IEnumerable, que é uma interface de somente leitura
        {
            try
            {
                throw new DataMisalignedException();    
                //return _context.Categorias.Include(c => c.Produtos).ToList(); // O metodo de extensao include permite carregar entidades relacionadas, nesse codigo EF, estamos obtendo todas as categorias e os produtos de cada categorias relaciona pela FK no banco

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação");
            }
        }

        [HttpGet] ///Define o verbo http do metodo Get()
        public ActionResult<IEnumerable<Categoria>> Get() ///Retorna uma ActionResult("Usamos quando queremos retornar um status code por exemplo"), e tambem um IEnumerable, que é uma interface de somente leitura
        {
            var categorias = _context.Categorias.AsNoTracking().ToList(); ///Lista todos os categorias da tabela categorias pelo EFCORE com o metodo ToList();
            if (categorias is null)
            {
                return NotFound("Categorias não encontradas"); //Retorna um status code 404 do tipo ActionResult
            }
            return categorias; ///Retorna a lista de produtos da interface IEnumerable
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")] ///Define o verbo http com um parametro id expecificado por dois pontos ":" 
        public ActionResult<Categoria> Get(int id) ///Retorna um ActionResult e recebe um id pelo parametro da rota "/Categorias/2" por exemplo, o id recebido seria "2"
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id); /// Manipula o parametro recebido pelo EFCORE
                if (categoria is null)
                {
                    return NotFound("Produto não encontrado");///Retorna um status code 404 do tipo ActionResult
                }
                return Ok(categoria); ///Retorna uma categoria
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação");
            }
           
        }



        [HttpPost] //Define o verbo http de request
        public ActionResult Post(Categoria categoria) //retorna um status code pelo ActionResult
        {
            try
            {
                if (categoria is null) return BadRequest(); //caso o posto receber o parametro como nulo, retorna um 404

                _context.Categorias.Add(categoria); //Utiliza da classe de contexto do EFCORE para adicionar uma categoria na tabela com o metodo Add
                _context.SaveChanges(); //Salva o estado do contexto no banco
                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria); //Aqui retornamos um status code 204 e no output realizamos uma chamada de retorna que esta com o name de ObterCategoria no caso nosso Get que recebo o parametro Id, assim conseguimos mostrar para o usuario como foi criado a categoria no banco
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação");
            }

        }


        [HttpPut("{id:int}")]  //Define o verbo http 
        public ActionResult Put(int id, Categoria categoria) //Retorna uma status code 
        {
            try
            {
                if (id != categoria.CategoriaId) //Caso o parametro id for diferente do id objeto parametro categoria que sera editado, retorna um 404
                {
                    return BadRequest();//Retorna status 404
                }

                _context.Entry(categoria).State = EntityState.Modified; //Modifica o estado do produto
                _context.SaveChanges(); //Salva o novo estado no banco

                return Ok(categoria); //Retorna status 200 e o corpo do produto
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
                var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id); //Retorna a categoria ou null se nao encontrar

                if (categoria is null)
                {
                    return NotFound("Produto não localizado"); //Retorna um 404
                }

                _context.Categorias.Remove(categoria); //Remove o estado do context da categoria
                _context.SaveChanges(); //Executa a remocao

                return Ok(categoria); //Retorna status code 200 e a categoria como corpo
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitação");
            }

        }
    }
}
