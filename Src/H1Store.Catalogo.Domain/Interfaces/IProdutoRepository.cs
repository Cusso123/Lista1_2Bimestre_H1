using H1Store.Catalogo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Domain.Interfaces
{
	public interface IProdutoRepository
	{
		IEnumerable<Produto> ObterTodosProdutos();
		Task<Produto> ObterProdutoPorCodigo(int id);
		Task<IEnumerable<Produto>> ObterPorNome(string nomeProduto);
		Task AdicionarProduto(Produto produto);
		Task AtualizarProduto(Produto produto);
		Task RemoverProduto(Produto produto);
		Task Ativar(Produto produto);
		Task Desativar(Produto produto);
		Task AlterarPreco(Produto produto, decimal valor);
		Task AtualizarEstoque(Produto produto, int quantidade);
	}
}
