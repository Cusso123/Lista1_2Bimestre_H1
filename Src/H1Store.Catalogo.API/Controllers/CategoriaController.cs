using H1Store.Catalogo.Application.Interfaces;
using H1Store.Catalogo.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace H1Store.Catalogo.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CategoriaController : ControllerBase
	{
		private readonly ICategoriaService _categoriaService;

		public CategoriaController(ICategoriaService categoriaService)
		{
			_categoriaService = categoriaService;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var categorias = await _categoriaService.ObterTodasCategorias();
			return Ok(categorias);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var categoria = await _categoriaService.ObterCategoriaPorCodigo(id);
			return Ok(categoria);
		}

		[HttpPost]
		public IActionResult Post(NovaCategoriaViewModel novoFornecedorViewModel)
		{
			_categoriaService.AdicionarCategoria(novoFornecedorViewModel);
			return Ok("Registro adicionado com sucesso!");
		}

		[HttpPut("{id}")]
		public IActionResult Put(int id, NovaCategoriaViewModel novoCategoriaViewModel)
		{
			novoCategoriaViewModel.Codigo = id;
			_categoriaService.AtualizarCategoria(novoCategoriaViewModel);

			return Ok("Registro atualizado com sucesso!");
		}

		[HttpPut("AtualizarRazaoSocial/{id}/{novaDescricao}")]
		public async Task<IActionResult> AlterarDescricao(int id, string novaDescricao)
		{
			await _categoriaService.AlterarDescricao(id, novaDescricao);

			return Ok("Descrição da categoria alterada com sucesso");
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			bool excluidoComSucesso = _categoriaService.RemoverCategoria(id);

			if (excluidoComSucesso)
			{
				return Ok("Registro excluído com sucesso!");
			}
			else
			{
				return NotFound("Registro não encontrado ou não pôde ser excluído.");
			}
		}
	}
}

