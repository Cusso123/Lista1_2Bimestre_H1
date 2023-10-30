using AutoMapper;
using H1Store.Catalogo.Data.Providers.MongoDb.Collections;
using H1Store.Catalogo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Store.Catalogo.Data.AutoMapper
{
	public class CollectionToDomain : Profile
	{
		public CollectionToDomain()
		{

			CreateMap<ProdutoCollection, Produto>()
			   .ConstructUsing(q => new Produto(q.Codigo, q.Nome, q.Descricao, q.Ativo, q.Valor, q.DataCadastro, q.QuantidadeEstoque));
		}
	}
}
