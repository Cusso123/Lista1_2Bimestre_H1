using AutoMapper;
using H1Store.Catalogo.Data.Providers.MongoDb.Collections;
using H1Store.Catalogo.Data.Providers.MongoDb.Interfaces;
using H1Store.Catalogo.Domain.Entities;
using H1Store.Catalogo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace H1Store.Catalogo.Data.Repository
{
	public class CategoriaRepository : ICategoriaRepository
	{
		private readonly string _categoriaCaminhoArquivo;

		#region Construtores Json

		public CategoriaRepository()
		{
			_categoriaCaminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), "FileJsonData", "categoria.json"); ;
		}

		#endregion

		#region Construtor MongoDB

		private readonly IMongoRepository<CategoriaCollection> _categoriaRepository;
		private readonly IMapper _mapper;

		public CategoriaRepository(IMongoRepository<CategoriaCollection> categoriaRepository, IMapper mapper)
		{
			_categoriaRepository = categoriaRepository;
			_mapper = mapper;
		}

		#endregion

		#region Funções do Arquivo 
		public IEnumerable<Categoria> ObterTodasCategorias()
		{
			var categoriaList = _categoriaRepository.FilterBy(filter => true);

			List<Categoria> lista = new List<Categoria>();
			foreach (var item in categoriaList)
			{
				lista.Add(new Categoria(item.Codigo, item.Descricao));
			}
			return lista;
		}

		public async Task<Categoria> ObterCategoriaPorCodigo(int id)
		{
			var buscaCategoria = _categoriaRepository.FilterBy(filter => filter.Codigo == id);
			var categoria = _mapper.Map<Categoria>(buscaCategoria.FirstOrDefault());
			return categoria;
		}

		public Task<IEnumerable<Categoria>> ObterPorCategoria(string nomeCategoria)
		{
			throw new NotImplementedException();
		}

		public async Task AlterarDescricao(Categoria categoria, string novaDescricao)
		{
			var buscaCategoria = _categoriaRepository.FilterBy(filter => filter.Codigo == categoria.Codigo);

			var categoriaDescricao = buscaCategoria.FirstOrDefault();

			categoriaDescricao.Descricao = categoria.Descricao;

			await _categoriaRepository.ReplaceOneAsync(_mapper.Map<CategoriaCollection>(categoriaDescricao));
		}

		public async Task AdicionarCategoria(Categoria categoria)
		{
			CategoriaCollection categoriaCollection = new CategoriaCollection();
			categoriaCollection.Codigo = categoria.Codigo;
			categoriaCollection.Descricao = categoria.Descricao;

			await _categoriaRepository.InsertOneAsync(categoriaCollection);
		}

		public async Task AtualizarCategoria(Categoria categoria)
		{
			var buscaFornecedor = _categoriaRepository.FilterBy(filter => filter.Codigo == categoria.Codigo);
			var fornecedorAtualizar = buscaFornecedor.FirstOrDefault();

			if (fornecedorAtualizar == null)
			{
				throw new ApplicationException("Produto não encontrado.");
			}

			fornecedorAtualizar.Codigo = categoria.Codigo;
			fornecedorAtualizar.Descricao = categoria.Descricao;

			await _categoriaRepository.ReplaceOneAsync(_mapper.Map<CategoriaCollection>(fornecedorAtualizar));
		}

		public bool RemoverCategoria(int id)
		{
			List<Categoria> categorias = LerCategoriasDoArquivo();
			var categoriaExistente = categorias.FirstOrDefault(p => p.Codigo == id);
			if (categoriaExistente != null)
			{
				categorias.Remove(categoriaExistente);
				EscreverCategoriaNoArquivo(categorias);
				return true;
			}
			else
			{
				return false;
			}
		}
		#endregion

		#region Métodos do Arquivo

		private List<Categoria> LerCategoriasDoArquivo()
		{
			if (!System.IO.File.Exists(_categoriaCaminhoArquivo))
			{
				return new List<Categoria>();
			}

			string json = System.IO.File.ReadAllText(_categoriaCaminhoArquivo);
			return JsonConvert.DeserializeObject<List<Categoria>>(json);
		}

		private int ObterProximoCodigoDisponivel()
		{
			List<Categoria> categorias = LerCategoriasDoArquivo();
			if (categorias.Any())
			{
				return categorias.Max(p => p.Codigo) + 1;
			}
			else
			{
				return 1;
			}
		}

		private void EscreverCategoriaNoArquivo(List<Categoria> categorias)
		{
			string json = JsonConvert.SerializeObject(categorias);
			System.IO.File.WriteAllText(_categoriaCaminhoArquivo, json);
		}

		#endregion
	}
}
