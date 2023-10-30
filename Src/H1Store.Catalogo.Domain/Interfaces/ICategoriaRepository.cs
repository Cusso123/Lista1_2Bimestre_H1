using H1Store.Catalogo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Domain.Interfaces
{
	public interface ICategoriaRepository
	{
		IEnumerable<Categoria> ObterTodasCategorias();
		Task<Categoria> ObterCategoriaPorCodigo(int id);
		Task<IEnumerable<Categoria>> ObterPorCategoria(string nomeCategoria);
		Task AdicionarCategoria(Categoria categoria);
		Task AtualizarCategoria(Categoria categoria);
		bool RemoverCategoria(int id);
		Task AlterarDescricao(Categoria categoria, string novaDescricao);
	}
}
