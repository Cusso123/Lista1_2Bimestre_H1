using H1Store.Catalogo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Domain.Interfaces
{
	public interface IFornecedorRepository
	{
		IEnumerable<Fornecedor> ObterTodosFornecedor();
		Task<Fornecedor> ObterFornecedorPorCodigo(int id);
		Task<IEnumerable<Fornecedor>> ObterPorFornecedor(string nomeFornecedor);
		Task AdicionarFornecedor(Fornecedor fornecedor);
		Task AtualizarFornecedor(Fornecedor fornecedor);
		bool RemoverFornecedor(int id);
		Task AlterarEmailContato(Fornecedor fornecedor, string novoEmail);
		Task AlterarRazaoSocial(Fornecedor fornecedor, string novaRazaoSocial);
		Task Ativar(Fornecedor fornecedor);
		Task Desativar(Fornecedor fornecedor);
	}
}
