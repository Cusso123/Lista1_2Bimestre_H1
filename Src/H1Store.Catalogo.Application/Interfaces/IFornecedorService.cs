using H1Store.Catalogo.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Application.Interfaces
{
	public interface IFornecedorService
	{
		Task<IEnumerable<FornecedorViewModel>> ObterTodosFornecedor();
		Task<FornecedorViewModel> ObterFornecedorPorCodigo(int id);
		Task<IEnumerable<FornecedorViewModel>> ObterPorFornecedor(string nomeFornecedor);
		Task AdicionarFornecedor(NovoFornecedorViewModel novoFornecedor);
		Task AtualizarFornecedor(NovoFornecedorViewModel novoFornecedor);
		bool RemoverFornecedor(int id);
		Task AlterarEmailContato(int id, string novoEmail);
		Task AlterarRazaoSocial(int id, string novaRazaoSocial);
		Task Ativar(int id);
		Task Desativar(int id);
	}
}
