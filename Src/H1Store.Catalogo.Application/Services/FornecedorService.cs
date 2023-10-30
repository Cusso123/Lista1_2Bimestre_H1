using AutoMapper;
using H1Store.Catalogo.Application.Interfaces;
using H1Store.Catalogo.Application.ViewModels;
using H1Store.Catalogo.Domain.Entities;
using H1Store.Catalogo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Application.Services
{
	public class FornecedorService : IFornecedorService
	{
		#region Construtores
		private readonly IFornecedorRepository _fornecedorRepository;
		private IMapper _mapper;

		public FornecedorService(IFornecedorRepository fornecedorRepository, IMapper mapper)
		{
			_fornecedorRepository = fornecedorRepository;
			_mapper = mapper;
		}
		#endregion


		#region Funções
		public async Task AdicionarFornecedor(NovoFornecedorViewModel novoFornecedorViewModel)
		{
			var novoFornecedor = _mapper.Map<Fornecedor>(novoFornecedorViewModel);
			await _fornecedorRepository.AdicionarFornecedor(novoFornecedor);
		}

		public async Task AlterarEmailContato(int id, string novoEmail)
		{
			var buscaFornecedor = await _fornecedorRepository.ObterFornecedorPorCodigo(id);

			if (buscaFornecedor == null)
			{
				throw new ApplicationException("Não é possível alterar o email de um fornecedor que não existe!");
			}

			buscaFornecedor.AlterarEmailContato(novoEmail);

			await _fornecedorRepository.AlterarEmailContato(buscaFornecedor, novoEmail);
		}

		public async Task AlterarRazaoSocial(int id, string novaRazaoSocial)
		{
			var buscaFornecedor = await _fornecedorRepository.ObterFornecedorPorCodigo(id);

			if (buscaFornecedor == null)
			{
				throw new ApplicationException("Não é possível alterar a razão social de um fornecedor que não existe!");
			}

			buscaFornecedor.AlterarRazaoSocial(novaRazaoSocial);

			await _fornecedorRepository.AlterarRazaoSocial(buscaFornecedor, novaRazaoSocial);
		}

		public async Task AtualizarFornecedor(NovoFornecedorViewModel novoFornecedorViewModel)
		{
			var fornecedor = _mapper.Map<Fornecedor>(novoFornecedorViewModel);
			await _fornecedorRepository.AtualizarFornecedor(fornecedor);
		}

		public bool RemoverFornecedor(int id)
		{
			bool excluidoComSucesso = _fornecedorRepository.RemoverFornecedor(id);

			if (excluidoComSucesso)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<IEnumerable<FornecedorViewModel>> ObterPorFornecedor(string nomeFornecedor)
		{
			if (string.IsNullOrWhiteSpace(nomeFornecedor))
			{
				return Enumerable.Empty<FornecedorViewModel>();
			}

			var fornecedores = await _fornecedorRepository.ObterPorFornecedor(nomeFornecedor);

			var fornecedoresViewModel = _mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);

			return fornecedoresViewModel;
		}

		public async Task<FornecedorViewModel> ObterFornecedorPorCodigo(int id)
		{
			var fornecedores = await _fornecedorRepository.ObterFornecedorPorCodigo(id);
			return _mapper.Map<FornecedorViewModel>(fornecedores);
		}

		public async Task<IEnumerable<FornecedorViewModel>> ObterTodosFornecedor()
		{
			return _mapper.Map<IEnumerable<FornecedorViewModel>>(_fornecedorRepository.ObterTodosFornecedor());
		}

		public async Task Ativar(int id)
		{
			var buscaFornecedor = await _fornecedorRepository.ObterFornecedorPorCodigo(id);

			if (buscaFornecedor == null)
				throw new ApplicationException("Não é possível ativar um fornecedor que não existe!");

			buscaFornecedor.Ativar();

			await _fornecedorRepository.Ativar(buscaFornecedor);
		}

		public async Task Desativar(int id)
		{
			var buscaFornecedor = await _fornecedorRepository.ObterFornecedorPorCodigo(id);

			if (buscaFornecedor == null)
				throw new ApplicationException("Não é possível desativar um fornecedor que não existe!");

			buscaFornecedor.Desativar();

			await _fornecedorRepository.Desativar(buscaFornecedor);
		}
		#endregion
	}
}
