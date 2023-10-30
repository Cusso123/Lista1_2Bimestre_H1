using H1Store.Catalogo.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Domain.Entities
{
	public class Produto
	{
		#region - Construtores
		public Produto(int codigo, string nome, string descricao, bool ativo, decimal valor, DateTime dataCadastro, int quantidadeestoque)
		{
			Codigo = codigo;
			Nome = nome;
			Descricao = descricao;
			Ativo = ativo;
			Valor = valor;
			DataCadastro = dataCadastro;
			QuantidadeEstoque = quantidadeestoque;
		}
		#endregion

		#region - Propriedades
		public int Codigo { get; private set; }
		public string Nome { get; private set; }
		public string Descricao { get; private set; }
		public bool Ativo { get; private set; }
		public decimal Valor { get; private set; }
		public DateTime DataCadastro { get; private set; }
		public int QuantidadeEstoque { get; private set; }

		#endregion

		#region - Comportamentos    

		public void Ativar()
		{
			Ativo = true;
		}

		public void Desativar()
		{
			Ativo = false;
		}

		public void AlterarDescricao(string novaDescricao)
		{
			Descricao = novaDescricao;
		}

		public void AlterarNome(string novoNome)
		{
			Nome = novoNome;
		}

		public void ReporEstoque(int quantidade)
		{
			QuantidadeEstoque += quantidade;
		}

		public bool PossuiEstoque(int quantidade) => QuantidadeEstoque >= quantidade;

		public void AlterarPreco(decimal valor)
		{
			Valor = valor;
		}

		#endregion
	}
}
