using AutoMapper;
using H1Store.Catalogo.Data.Providers.MongoDb.Collections;
using H1Store.Catalogo.Data.Providers.MongoDb.Interfaces;
using H1Store.Catalogo.Domain.Entities;
using H1Store.Catalogo.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace H1Store.Catalogo.Data.Repository
{
	public class ProdutoRepository : IProdutoRepository
	{
		private readonly IMongoRepository<ProdutoCollection> _produtoRepository;

		private readonly string _produtoCaminhoArquivo;
		private readonly IMapper _mapper;

		#region - Construtores
		public ProdutoRepository(IMongoRepository<ProdutoCollection> produtoRepository, IMapper mapper)
		{
			_produtoRepository = produtoRepository;
			_mapper = mapper;
		}
		#endregion

		#region Funções do Arquivo      

		public IEnumerable<Produto> ObterTodosProdutos()
		{
			var produtoList = _produtoRepository.FilterBy(filter => true);

			List<Produto> lista = new List<Produto>();
			foreach (var item in produtoList)
			{
				lista.Add(new Produto(item.Codigo, item.Nome, item.Descricao, item.Ativo, item.Valor, item.DataCadastro, item.QuantidadeEstoque));
			}
			return lista;
		}

		public async Task<Produto> ObterProdutoPorCodigo(int id)
		{
			var buscaProduto = _produtoRepository.FilterBy(filter => filter.Codigo == id);
			var produto = _mapper.Map<Produto>(buscaProduto.FirstOrDefault());
			return produto;
		}

		public async Task<IEnumerable<Produto>> ObterPorNome(string nomeProduto)
		{
			var produtosEncontrados = _produtoRepository.FilterBy(filter => filter.Nome.Contains(nomeProduto));

			return _mapper.Map<IEnumerable<Produto>>(produtosEncontrados);
		}

		public async Task AdicionarProduto(Produto produto)
		{
			ProdutoCollection produtoCollection = new ProdutoCollection();
			produtoCollection.Codigo = produto.Codigo;
			produtoCollection.Nome = produto.Nome;
			produtoCollection.Descricao = produto.Descricao;
			produtoCollection.Ativo = produto.Ativo;
			produtoCollection.Valor = produto.Valor;
			produtoCollection.DataCadastro = produto.DataCadastro;
			produtoCollection.QuantidadeEstoque = produto.QuantidadeEstoque;

			await _produtoRepository.InsertOneAsync(produtoCollection);
		}

		public async Task AtualizarProduto(Produto produto)
		{
			var buscaProduto = _produtoRepository.FilterBy(filter => filter.Codigo == produto.Codigo);
			var produtoAtualizar = buscaProduto.FirstOrDefault();

			if (produtoAtualizar == null)
			{
				throw new ApplicationException("Produto não encontrado.");
			}

			produtoAtualizar.Nome = produto.Nome;
			produtoAtualizar.Descricao = produto.Descricao;
			produtoAtualizar.Ativo = produto.Ativo;
			produtoAtualizar.Valor = produto.Valor;
			produtoAtualizar.QuantidadeEstoque = produto.QuantidadeEstoque;

			await _produtoRepository.ReplaceOneAsync(_mapper.Map<ProdutoCollection>(produtoAtualizar));
		}

		public async Task Ativar(Produto produto)
		{
			var buscaProduto = _produtoRepository.FilterBy(filter => filter.Codigo == produto.Codigo);

			var produtoAtivar = buscaProduto.FirstOrDefault();

			produtoAtivar.Ativo = produto.Ativo;

			await _produtoRepository.ReplaceOneAsync(_mapper.Map<ProdutoCollection>(produtoAtivar));
		}

		public async Task Desativar(Produto produto)
		{
			var buscaProduto = _produtoRepository.FilterBy(filter => filter.Codigo == produto.Codigo);

			var produtoDesativar = buscaProduto.FirstOrDefault();

			produtoDesativar.Ativo = produto.Ativo;

			await _produtoRepository.ReplaceOneAsync(_mapper.Map<ProdutoCollection>(produtoDesativar));
		}

		public async Task AlterarPreco(Produto produto, decimal valor)
		{
			var buscaProduto = _produtoRepository.FilterBy(filter => filter.Codigo == produto.Codigo);

			var produtoPreco = buscaProduto.FirstOrDefault();

			produtoPreco.Valor = produto.Valor;

			await _produtoRepository.ReplaceOneAsync(_mapper.Map<ProdutoCollection>(produtoPreco));
		}

		public async Task AtualizarEstoque(Produto produto, int quantidade)
		{
			var buscaProduto = _produtoRepository.FilterBy(filter => filter.Codigo == produto.Codigo);

			var produtoEstoque = buscaProduto.FirstOrDefault();

			produtoEstoque.QuantidadeEstoque = produto.QuantidadeEstoque;

			await _produtoRepository.ReplaceOneAsync(_mapper.Map<ProdutoCollection>(produtoEstoque));
		}

		public async Task RemoverProduto(Produto produto)
		{
			var buscaProduto = _produtoRepository.FilterBy(filter => filter.Codigo == produto.Codigo);
			var produtoAtualizar = buscaProduto.FirstOrDefault();

			if (produtoAtualizar == null)
			{
				throw new ApplicationException("Produto não encontrado.");
			}

			var filtro = Builders<ProdutoCollection>.Filter.Eq(p => p.Codigo, produto.Codigo);

			await _produtoRepository.DeleteOneAsync(filtro);
		}



		#endregion

		#region Métodos do Arquivo
		private List<Produto> LerProdutosDoArquivo()
		{
			if (!System.IO.File.Exists(_produtoCaminhoArquivo))
			{
				return new List<Produto>();
			}

			string json = System.IO.File.ReadAllText(_produtoCaminhoArquivo);
			return JsonConvert.DeserializeObject<List<Produto>>(json);
		}

		private int ObterProximoCodigoDisponivel()
		{
			List<Produto> produtos = LerProdutosDoArquivo();
			if (produtos.Any())
			{
				return produtos.Max(p => p.Codigo) + 1;
			}
			else
			{
				return 1;
			}
		}

		private void EscreverProdutosNoArquivo(List<Produto> produtos)
		{
			string json = JsonConvert.SerializeObject(produtos);
			System.IO.File.WriteAllText(_produtoCaminhoArquivo, json);
		}
		#endregion
	}
}
