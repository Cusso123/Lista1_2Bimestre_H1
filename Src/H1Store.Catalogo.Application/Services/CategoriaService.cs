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
	
    public class CategoriaService : ICategoriaService
	{
		#region Construtores
		private readonly ICategoriaRepository _categoriaRepository;
		private IMapper _mapper;

		public CategoriaService(ICategoriaRepository categoriaRepository, IMapper mapper)
		{
			_categoriaRepository = categoriaRepository;
			_mapper = mapper;
		}
		#endregion

		#region Funções
		public async Task AdicionarCategoria(NovaCategoriaViewModel novaCategoriaViewModel)
		{
			var novaCategoria = _mapper.Map<Categoria>(novaCategoriaViewModel);
			await _categoriaRepository.AdicionarCategoria(novaCategoria);
		}

		public async Task AlterarDescricao(int id, string novaDescricao)
		{
			var buscaCategoria = await _categoriaRepository.ObterCategoriaPorCodigo(id);

			if (buscaCategoria == null)
			{
				throw new ApplicationException("Não é possível alterar a descrição de uma categoria que não existe!");
			}

			buscaCategoria.AlterarDescricao(novaDescricao);

			await _categoriaRepository.AlterarDescricao(buscaCategoria, novaDescricao);
		}


		public async Task AtualizarCategoria(NovaCategoriaViewModel novaCategoriaViewModel)
		{
			var categoria = _mapper.Map<Categoria>(novaCategoriaViewModel);
			await _categoriaRepository.AtualizarCategoria(categoria);
		}

		public bool RemoverCategoria(int id)
		{
			bool excluidoComSucesso = _categoriaRepository.RemoverCategoria(id);

			if (excluidoComSucesso)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public Task<IEnumerable<CategoriaViewModel>> ObterPorCategoria(int codigo)
		{
			throw new NotImplementedException();
		}

		public async Task<CategoriaViewModel> ObterCategoriaPorCodigo(int id)
		{
			var categorias = await _categoriaRepository.ObterCategoriaPorCodigo(id);
			return _mapper.Map<CategoriaViewModel>(categorias);
		}

		public async Task<IEnumerable<CategoriaViewModel>> ObterTodasCategorias()
		{
			return _mapper.Map<IEnumerable<CategoriaViewModel>>(_categoriaRepository.ObterTodasCategorias());
		}
		#endregion
	}
}

