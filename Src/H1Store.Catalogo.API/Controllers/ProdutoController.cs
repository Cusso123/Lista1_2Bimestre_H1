using H1Store.Catalogo.Application.Interfaces;
using H1Store.Catalogo.Application.ViewModels;
using H1Store.Catalogo.Data.Providers.MongoDb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Xml.Linq;

namespace H1Store.Catalogo.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProdutoController : ControllerBase
	{
		private readonly IProdutoService _produtoService;

		public ProdutoController(IProdutoService produtoService)
		{
			_produtoService = produtoService;
		}

		[HttpGet(Name = "ObterTodos")]
		public IActionResult Get()
		{
			return Ok(_produtoService.ObterTodosProdutos());
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var produto = await _produtoService.ObterProdutoPorCodigo(id);
			return Ok(produto);
		}

		[HttpGet("BuscarPorNome/{nome}")]
		public async Task<IActionResult> ObterPorNome(string nome)
		{
			var produtos = await _produtoService.ObterPorNome(nome);

			if (produtos.Any())
			{
				return Ok(produtos);
			}
			else
			{
				return NotFound("Nenhum produto encontrado com o nome especificado.");
			}
		}

		[HttpPost]
		public IActionResult Post(NovoProdutoViewModel novoProdutoViewModel)
		{
			_produtoService.AdicionarProduto(novoProdutoViewModel);

			return Ok("Registro adicionado com sucesso!");
		}

		[HttpPut("{id}")]
		public IActionResult Put(int id, NovoProdutoViewModel novoProdutoViewModel)
		{
			novoProdutoViewModel.Codigo = id;
			_produtoService.AtualizarProduto(novoProdutoViewModel);

			return Ok("Registro atualizado com sucesso!");
		}

		[HttpPut("AtualizarEstoque/{id}/{quantidade}")]
		public async Task<IActionResult> AtualizaEstoque(int id, int quantidade)
		{
			await _produtoService.AtualizarEstoque(id, quantidade);

			return Ok("Estoque do produto alterado com sucesso");
		}

		[HttpPut("AlterarPreco/{id}/{novoPreco}")]
		public async Task<IActionResult> AlterarPreco(int id, decimal novoPreco)
		{
			await _produtoService.AlterarPreco(id, novoPreco);

			return Ok("Preço do produto alterado com sucesso");
		}

		[HttpPut]
		[Route("Ativar/{id}")]
		public async Task<IActionResult> Ativa(int id)
		{
			await _produtoService.Ativar(id);
			return Ok("Produto ativado com sucesso");
		}

		[HttpPut]
		[Route("Desativar/{id}")]
		public async Task<IActionResult> Desativa(int id)
		{
			await _produtoService.Desativar(id);
			return Ok("Produto desativado com sucesso");
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _produtoService.RemoverProduto(id);
			return Ok("Produto deletado com sucesso!");
		}

	}
}
