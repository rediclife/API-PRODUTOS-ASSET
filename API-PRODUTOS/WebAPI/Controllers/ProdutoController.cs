using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.InterfaceServices;
using Domain.Services;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProdutoController : ControllerBase
    {
        private readonly IMapper _IMapper;
        private readonly IProduto _IProduto;
        private readonly IServiceProduto _IServiceProduto;

        public ProdutoController(IMapper IMapper, IProduto IProduto, IServiceProduto IServiceProduto)
        {
            _IMapper = IMapper;
            _IProduto = IProduto;
            _IServiceProduto = IServiceProduto;
        }

        /// <summary>
        /// Cadastrar um produto
        /// </summary>
        /// <remarks>
        /// {"id":"int", "codigoEan":"string", "descricao":"string", "valor":"decimal", "categoriaProdutoId":"int", "UserId":"string"}
        /// </remarks>
        /// <param name="produto">Dados do Produto</param>
        /// <returns>Objeto recém-criado</returns>
        /// <response code="201">Prodto criado com sucesso</response>
        /// <response code="400">Retorna erros de validação</response>
        /// <response code="500">Retorna caso erros ocorram</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProdutoDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModelView<Notifies>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Add(ProdutoDTO produto)
        {

            try
            {
                produto.UserId = User.FindFirst("idUsuario").Value;
                Produto produtoMap = _IMapper.Map<Produto>(produto);
                await _IServiceProduto.Adicionar(produtoMap);

                if (produtoMap.Notificacoes.Count > 0)
                {
                    return BadRequest(new ResponseModelView<Notifies>(false, "Falha na validação dos dados", produtoMap.Notificacoes));
                }

                return Created("Add", new ResponseModelView<ProdutoDTO>(true, "Produto adicionado com sucesso", _IMapper.Map<ProdutoDTO>(produtoMap)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }


        }

        /// <summary>
        /// Obter todos os produto
        /// </summary>
        /// <returns>Lista de produtos</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Produto não encontrado</response>
        /// <response code="400">Retorna caso erros ocorram</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ListAll()
        {

            try
            {
                var produtos = await _IProduto.ListAll();
                var produtosMap = _IMapper.Map<List<ProdutoDTO>>(produtos);

                if (produtosMap == null)
                {
                    return NotFound(new ResponseModelView(false, "A pesquisa não retornou resultados"));
                }

                return Ok(new ResponseModelView<ProdutoDTO>(true, "Consulta realizada com sucesso", produtosMap));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }
        }

        /// <summary>
        /// Obter todas os produtos com paginação indicando a pagina e quantidade a ser exibida
        /// </summary>
        /// <param name="page">Número da pagina</param>
        /// <param name="pageSize">Tamanho / Quantidade registro por pagina</param>
        /// <returns>Lista de produtos com paginação</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("GetAllPagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllPagination(int page = 1, int pageSize = 10)
        {
            try
            {
                var produtos = await _IProduto.ListAll();
                var produtosMap = _IMapper.Map<List<ProdutoDTO>>(produtos);

                if (produtosMap == null)
                {
                    return NotFound(new ResponseModelView(false, "A pesquisa não retornou resultados"));
                }

                var totalCount = produtosMap.Count;
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
                var produtosPorPagina = _IMapper.Map<List<ProdutoDTO>>(produtosMap.Skip((page - 1) * pageSize).Take(pageSize).ToList());

                return Ok(new ResponseModelView<ProdutoDTO>(true, "Consulta realizada com sucesso", produtosPorPagina));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }

        }

        //[HttpGet("GetById")]
        /// <summary>
        /// Obter um produto por Id
        /// </summary>
        /// <param name="id">Identificador do produto</param>
        /// <returns>Dados do produto</returns>
        /// <response code="200">Produto consutado com sucesso</response>
        /// <response code="404">Produto não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
        {

            try
            {
                Produto produto = await _IProduto.GetEntityById(id);
                var produtoMap = _IMapper.Map<ProdutoDTO>(produto);

                if (produtoMap == null)
                {
                    return NotFound(new ResponseModelView(false, "A pesquisa não retornou resultados"));
                }

                return Ok(new ResponseModelView<ProdutoDTO>(true, "Consulta realizada com sucesso", _IMapper.Map<ProdutoDTO>(produtoMap)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }
        }

        /// <summary>
        /// Atualizar um produto
        /// </summary>
        /// <remarks>
        /// {"id":"int", "codigoEan":"string", "descricao":"string", "valor":"decimal", "categoriaProdutoId":"int", "UserId":"string"}
        /// </remarks>
        /// <param name="id">Identificador do produto</param>
        /// <param name="produto">Dados do produto</param>
        /// <returns>Nada.</returns>
        /// <response code="400">Retorna erros de validação</response>
        /// <response code="404">Produto não encontrado</response>
        /// <response code="200">Produto alterado com sucesso</response>
        /// <response code="500">Retorna caso erros ocorram</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProdutoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModelView<Notifies>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(int id, ProdutoDTO produto)
        {
            
            try
            {
                if (id != produto.Id)
                {
                    return BadRequest(new ResponseModelView(false, "Propriedade Id não pode ser alterada."));
                }

                Produto produtoMap = _IMapper.Map<Produto>(produto);
                await _IServiceProduto.Atualizar(produtoMap);


                if (produtoMap.Notificacoes.Count > 0)
                {
                    return BadRequest(new ResponseModelView<Notifies>(false, "Falha na validação dos dados", produtoMap.Notificacoes));
                }

                return Ok(new ResponseModelView<ProdutoDTO>(true, "Produto alterado com sucesso", _IMapper.Map<ProdutoDTO>(produtoMap)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }
        }

        /// <summary>
        /// Excluir um produto
        /// </summary>
        /// <remarks>
        /// {"id":"int", "codigoEan":"string", "descricao":"string", "valor":"decimal", "categoriaProdutoId":"int", "UserId":"string"}
        /// </remarks>
        /// <param name="produto">Dados do produto</param>
        /// <returns>Nada.</returns>
        /// <response code="404">Produto não encontrado</response>
        /// <response code="204">Produto excluido com sucesso</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete]
        public async Task<ActionResult> Delete(ProdutoDTO produto)
        {
            
            try
            {
                var produtoMap = _IMapper.Map<Produto>(produto);
                await _IProduto.Delete(produtoMap);

                if (produtoMap.Notificacoes.Count > 0)
                {
                    return BadRequest(new ResponseModelView<Notifies>(false, "Falha na validação dos dados", produtoMap.Notificacoes));
                }

                return Ok(new ResponseModelView(true, "Produto deletado com sucesso"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }
        }

        /// <summary>
        /// Excluir um produto por Id
        /// </summary>
        /// <param name="id">Identificador do produto</param>
        /// <returns>Nada</returns>
        /// <response code="404">Produto não encontrada</response>
        /// <response code="204">Produto excluido com sucesso</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _IProduto.Delete(id);

                return Ok(new ResponseModelView(true, "Produto deletado com sucesso"));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }
        }  
    }
}
