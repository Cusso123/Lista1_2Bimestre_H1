using H1Store.Catalogo.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Application.Interfaces
{
	public interface ICategoriaService
	{
		Task<IEnumerable<CategoriaViewModel>> ObterTodasCategorias();
		Task<CategoriaViewModel> ObterCategoriaPorCodigo(int id);
		Task<IEnumerable<CategoriaViewModel>> ObterPorCategoria(int codigo);
		Task AdicionarCategoria(NovaCategoriaViewModel produto);
		Task AtualizarCategoria(NovaCategoriaViewModel produto);
		bool RemoverCategoria(int id);
		Task AlterarDescricao(int id, string novaDescricao);
	}
}
