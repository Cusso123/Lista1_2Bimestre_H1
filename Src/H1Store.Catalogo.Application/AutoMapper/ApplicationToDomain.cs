using AutoMapper;
using H1Store.Catalogo.Application.ViewModels;
using H1Store.Catalogo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Application.AutoMapper
{
	public class ApplicationToDomain : Profile
	{
		public ApplicationToDomain()
		{
			CreateMap<NovoProdutoViewModel, Produto>()
			   .ConstructUsing(q => new Produto(q.Codigo, q.Nome, q.Descricao, q.Ativo, q.Valor, DateTime.Now, q.QuantidadeEstoque));
		}
	}
}
