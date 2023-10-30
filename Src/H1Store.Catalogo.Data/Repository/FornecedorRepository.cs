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
	
    public class FornecedorRepository : IFornecedorRepository
	{

		#region Construtor Json

		private readonly string _fornecedorCaminhoArquivo;

		public FornecedorRepository()
		{
			_fornecedorCaminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), "FileJsonData", "fornecedor.json"); ;
		}

		#endregion

		#region Construtor MongoDB

		private readonly IMongoRepository<FornecedorCollection> _fornecedorRepository;
		private readonly IMapper _mapper;

		public FornecedorRepository(IMongoRepository<FornecedorCollection> fornecedorRepository, IMapper mapper)
		{
			_fornecedorRepository = fornecedorRepository;
			_mapper = mapper;
		}
		#endregion

		#region Funções do Arquivo 

		public async Task AdicionarFornecedor(Fornecedor fornecedor)
		{
			FornecedorCollection categoriaCollection = new FornecedorCollection();
			categoriaCollection.Codigo = fornecedor.Codigo;
			categoriaCollection.RazaoSocial = fornecedor.RazaoSocial;
			categoriaCollection.CNPJ = fornecedor.CNPJ;
			categoriaCollection.Ativo = fornecedor.Ativo;
			categoriaCollection.DataCadastro = fornecedor.DataCadastro;
			categoriaCollection.EmailContato = fornecedor.EmailContato;

			await _fornecedorRepository.InsertOneAsync(categoriaCollection);
		}

		public async Task AtualizarFornecedor(Fornecedor fornecedor)
		{
			var buscaFornecedor = _fornecedorRepository.FilterBy(filter => filter.Codigo == fornecedor.Codigo);
			var fornecedorAtualizar = buscaFornecedor.FirstOrDefault();

			if (fornecedorAtualizar == null)
			{
				throw new ApplicationException("Produto não encontrado.");
			}

			fornecedorAtualizar.Codigo = fornecedor.Codigo;
			fornecedorAtualizar.RazaoSocial = fornecedor.RazaoSocial;
			fornecedorAtualizar.CNPJ = fornecedor.CNPJ;
			fornecedorAtualizar.Ativo = fornecedor.Ativo;
			fornecedorAtualizar.DataCadastro = fornecedor.DataCadastro;
			fornecedorAtualizar.EmailContato = fornecedor.EmailContato;

			await _fornecedorRepository.ReplaceOneAsync(_mapper.Map<FornecedorCollection>(fornecedorAtualizar));
		}

		public bool RemoverFornecedor(int id)
		{
			List<Fornecedor> fornecedores = LerFornecedoresDoArquivo();
			var fornecedorExistente = fornecedores.FirstOrDefault(p => p.Codigo == id);
			if (fornecedorExistente != null)
			{
				fornecedores.Remove(fornecedorExistente);
				EscreverFornecedorNoArquivo(fornecedores);
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task AlterarEmailContato(Fornecedor fornecedor, string novoEmail)
		{
			var buscaFornecedor = _fornecedorRepository.FilterBy(filter => filter.Codigo == fornecedor.Codigo);

			var fornecedorEmail = buscaFornecedor.FirstOrDefault();

			fornecedorEmail.EmailContato = fornecedor.EmailContato;

			await _fornecedorRepository.ReplaceOneAsync(_mapper.Map<FornecedorCollection>(fornecedorEmail));
		}

		public async Task AlterarRazaoSocial(Fornecedor fornecedor, string novaRazaoSocial)
		{
			var buscaFornecedor = _fornecedorRepository.FilterBy(filter => filter.Codigo == fornecedor.Codigo);

			var fornecedorRazaoSocial = buscaFornecedor.FirstOrDefault();

			fornecedorRazaoSocial.EmailContato = fornecedor.EmailContato;

			await _fornecedorRepository.ReplaceOneAsync(_mapper.Map<FornecedorCollection>(fornecedorRazaoSocial));
		}

		public Task<IEnumerable<Fornecedor>> ObterPorFornecedor(string nomeFornecedor)
		{
			var fornecedoresEncontrados = _fornecedorRepository.FilterBy(filter => filter.RazaoSocial.Contains(nomeFornecedor));

			return (Task<IEnumerable<Fornecedor>>)_mapper.Map<IEnumerable<Fornecedor>>(fornecedoresEncontrados);
		}

		public async Task<Fornecedor> ObterFornecedorPorCodigo(int id)
		{
			var buscaFornecedor = _fornecedorRepository.FilterBy(filter => filter.Codigo == id);
			var fornecedor = _mapper.Map<Fornecedor>(buscaFornecedor.FirstOrDefault());
			return fornecedor;
		}

		public IEnumerable<Fornecedor> ObterTodosFornecedor()
		{
			var fornecedorList = _fornecedorRepository.FilterBy(filter => true);

			List<Fornecedor> lista = new List<Fornecedor>();
			foreach (var item in fornecedorList)
			{
				lista.Add(new Fornecedor(item.Codigo, item.RazaoSocial, item.CNPJ, item.Ativo, item.DataCadastro, item.EmailContato));
			}
			return lista;
		}

		public async Task Ativar(Fornecedor fornecedor)
		{
			var buscaFornecedor = _fornecedorRepository.FilterBy(filter => filter.Codigo == fornecedor.Codigo);

			var fornecedorAtivar = buscaFornecedor.FirstOrDefault();

			fornecedorAtivar.Ativo = fornecedor.Ativo;

			await _fornecedorRepository.ReplaceOneAsync(_mapper.Map<FornecedorCollection>(fornecedorAtivar));
		}

		public async Task Desativar(Fornecedor fornecedor)
		{
			var buscaFornecedor = _fornecedorRepository.FilterBy(filter => filter.Codigo == fornecedor.Codigo);

			var fornecedorDesativar = buscaFornecedor.FirstOrDefault();

			fornecedorDesativar.Ativo = fornecedor.Ativo;

			await _fornecedorRepository.ReplaceOneAsync(_mapper.Map<FornecedorCollection>(fornecedorDesativar));
		}
		#endregion

		#region Métodos do Arquivo
		private List<Fornecedor> LerFornecedoresDoArquivo()
		{
			if (!System.IO.File.Exists(_fornecedorCaminhoArquivo))
			{
				return new List<Fornecedor>();
			}

			string json = System.IO.File.ReadAllText(_fornecedorCaminhoArquivo);
			return JsonConvert.DeserializeObject<List<Fornecedor>>(json);
		}

		private int ObterProximoCodigoDisponivel()
		{
			List<Fornecedor> fornecedores = LerFornecedoresDoArquivo();
			if (fornecedores.Any())
			{
				return fornecedores.Max(p => p.Codigo) + 1;
			}
			else
			{
				return 1;
			}
		}

		private void EscreverFornecedorNoArquivo(List<Fornecedor> fornecedores)
		{
			string json = JsonConvert.SerializeObject(fornecedores);
			System.IO.File.WriteAllText(_fornecedorCaminhoArquivo, json);
		}
		#endregion
	}
}

