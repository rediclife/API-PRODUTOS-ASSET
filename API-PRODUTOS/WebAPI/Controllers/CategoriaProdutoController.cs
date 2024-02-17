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
    //[Authorize]
    public class CategoriaProdutoController : ControllerBase
    {
        private readonly IMapper _IMapper;
        private readonly ICategoriaProduto _ICategoriaProd;
        private readonly IServiceCategoriaProduto _IServiceCategoriaProd;

        public CategoriaProdutoController(IMapper IMapper, ICategoriaProduto ICategoriaProd, IServiceCategoriaProduto IServiceCategoriaProd)
        {
            _IMapper = IMapper;
            _ICategoriaProd = ICategoriaProd;
            _IServiceCategoriaProd = IServiceCategoriaProd;
        }

        /// <summary>
        /// Cadastrar uma categoria de produto
        /// </summary>
        /// <remarks>
        /// {"id":"int","descricao":"string","UserId":"string"}
        /// </remarks>
        /// <param name="categoriaProd">Dados da Categoria</param>
        /// <returns>Objeto recém-criado</returns>
        /// <response code="201">Categoria criada com sucesso</response>
        /// <response code="400">Retorna erros de validação</response>
        /// <response code="500">Retorna caso erros ocorram</response>
        [HttpPost]
        [ProducesResponseType(typeof(CategoriaProdutoDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModelView<Notifies>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Add(CategoriaProdutoDTO categoriaProd)
        {
            try
            {
                categoriaProd.UserId = User.FindFirst("idUsuario").Value;
                CategoriaProduto categoriaProdMap = _IMapper.Map<CategoriaProduto>(categoriaProd);
                await _IServiceCategoriaProd.Adicionar(categoriaProdMap);

                if (categoriaProdMap.Notificacoes.Count > 0)
                {
                    return BadRequest(new ResponseModelView<Notifies>(false, "Falha na validação dos dados", categoriaProdMap.Notificacoes));
                }

                return Created("Add", new ResponseModelView<CategoriaProdutoDTO>(true, "Categoria adicionada com sucesso", _IMapper.Map<CategoriaProdutoDTO>(categoriaProdMap)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }

        }

        /// <summary>
        /// Obter todas as categorias de produto
        /// </summary>
        /// <returns>Lista de categorias</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        /// /// <response code="400">Retorna caso erros ocorram</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ListAll()
        {
            try
            {
                var categoriaProd = await _ICategoriaProd.ListAll();
                var categoriaProdMap = _IMapper.Map<List<CategoriaProdutoDTO>>(categoriaProd);

                if (categoriaProdMap == null)
                {
                    return NotFound(new ResponseModelView(false, "A pesquisa não retornou resultados"));
                }

                return Ok(new ResponseModelView<CategoriaProdutoDTO>(true, "Consulta realizada com sucesso", categoriaProdMap));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }


        }

        /// <summary>
        /// Obter uma categoria por Id
        /// </summary>
        /// <param name="id">Identificador da categoria</param>
        /// <returns>Dados da categoria</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        /// <response code="400">Retorna caso erros ocorram</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                CategoriaProduto categoriaProd = await _ICategoriaProd.GetEntityById(id);
                var categoriaProdMap = _IMapper.Map<CategoriaProdutoDTO>(categoriaProd);

                if (categoriaProd == null)
                {
                    return NotFound(new ResponseModelView(false, "A pesquisa não retornou resultados"));
                }
                else if (categoriaProd.Notificacoes.Count > 0)
                {
                    return BadRequest(new ResponseModelView<Notifies>(false, "Falha na validação dos dados", categoriaProd.Notificacoes));
                }


                return Ok(new ResponseModelView<CategoriaProdutoDTO>(true, "Consulta realizada com sucesso", _IMapper.Map<CategoriaProdutoDTO>(categoriaProdMap)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }


        }

        /// <summary>
        /// Obter todas as categorias de produto com paginação indicando a pagina e quantidade a ser exibida
        /// </summary>
        /// <param name="page">Número da pagina</param>
        /// <param name="pageSize">Tamanho / Quantidade registro por pagina</param>
        /// <returns>Lista de categorias com paginação</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("GetAllPagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllPagination(int page = 1, int pageSize = 10)
        {
            try
            {
                var categoriaProdes = await _ICategoriaProd.ListAll();
                var categoriaProdesMap = _IMapper.Map<List<CategoriaProdutoDTO>>(categoriaProdes);

                if (categoriaProdesMap == null)
                {
                    return NotFound(new ResponseModelView(false, "A pesquisa não retornou resultados"));
                }

                var totalCount = categoriaProdesMap.Count;


                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
                var categoriaProdesPorPagina = _IMapper.Map<List<CategoriaProdutoDTO>>(categoriaProdesMap.Skip((page - 1) * pageSize).Take(pageSize).ToList());

                return Ok(new ResponseModelView<CategoriaProdutoDTO>(true, "Consulta realizada com sucesso", categoriaProdesPorPagina));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }

        }

        /// <summary>
        /// Atualizar uma categoria
        /// </summary>
        /// <remarks>
        /// {"id":"int","descricao":"string","UserId":"string"}
        /// </remarks>
        /// <param name="id">Identificador da Categoria</param>
        /// <param name="categoriaProd">Dados da Categoria</param>
        /// <returns>Nada.</returns>
        /// <response code="400">Retorna erros de validação</response>
        /// <response code="404">Categoria não encontrada</response>
        /// <response code="200">Categoria alterada com sucesso</response>
        /// <response code="500">Retorna caso erros ocorram</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CategoriaProdutoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModelView<Notifies>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(int id, CategoriaProdutoDTO categoriaProd)
        {
            try
            {
                if (id != categoriaProd.Id)
                {
                    return BadRequest(new ResponseModelView(false, "Propriedade Id não pode ser alterada."));
                }

                var categoriaProdMap = _IMapper.Map<CategoriaProduto>(categoriaProd);
                await _IServiceCategoriaProd.Atualizar(categoriaProdMap);

                if (categoriaProdMap.Notificacoes.Count > 0)
                {
                    return BadRequest(new ResponseModelView<Notifies>(false, "Falha na validação dos dados.", categoriaProdMap.Notificacoes));
                }

                return Ok(new ResponseModelView<CategoriaProdutoDTO>(true, "Categoria alterada com sucesso.", _IMapper.Map<CategoriaProdutoDTO>(categoriaProdMap)));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }
            
        }

        /// <summary>
        /// Excluir uma categoria
        /// </summary>
        /// <remarks>
        /// {"id":"int","descricao":"string","UserId":"string"}
        /// </remarks>
        /// <param name="categoriaProd">Dados da Categoria</param>
        /// <returns>Nada.</returns>
        /// <response code="404">Categoria não encontrada</response>
        /// <response code="204">Categoria excluida com sucesso</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(CategoriaProdutoDTO categoriaProd)
        {
            try
            {
                var categoriaProdMap = _IMapper.Map<CategoriaProduto>(categoriaProd);
                await _ICategoriaProd.Delete(categoriaProdMap);

                if (categoriaProdMap.Notificacoes.Count > 0)
                {
                    return BadRequest(new ResponseModelView<Notifies>(false, "Falha na validação dos dados", categoriaProdMap.Notificacoes));
                }

                return Ok(new ResponseModelView(true, "Categoria deletada com sucesso"));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }

            
        }

        /// <summary>
        /// Excluir uma categoria por Id
        /// </summary>
        /// <param name="id">Identificador da Categoria</param>
        /// <returns>Nada</returns>
        /// <response code="404">Categoria não encontrada</response>
        /// <response code="204">Categoria excluida com sucesso</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var categoriaProd = await _IServiceCategoriaProd.ObterPorId(id);
                if (categoriaProd == null) 
                {
                    return BadRequest(new ResponseModelView(false, "Categoria não encontrada: Id "));
                }
                else if (categoriaProd.Produtos.Count > 0)
                {
                    return BadRequest(new ResponseModelView(false, "Exclusão não permitida: A categoria possui produtos relacionados"));
                }

                await _ICategoriaProd.Delete(id);
                
                return Ok(new ResponseModelView(true, "Categoria deletada com sucesso"));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModelView(false, "Erro: " + ex.Message));
            }


        }
    }
}
