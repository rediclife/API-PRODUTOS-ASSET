using Domain.Interfaces;
using Domain.Interfaces.InterfaceServices;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ServiceProduto : IServiceProduto
    {
        private readonly IProduto _IProduto;

        public ServiceProduto(IProduto IProduto)
        {
            _IProduto = IProduto;
        }

        public async Task Adicionar(Produto Objeto)
        {
            var validaDescricao = Objeto.ValidarPropriedadeString(Objeto.Descricao, "Descricao");
            if (validaDescricao)
            {
                var lstProdutos = await _IProduto.ListarProdutos(p => p.CodigoEan == Objeto.CodigoEan);

                if(lstProdutos.Count > 0)
                {
                    Objeto.AdicionarNotificacao("O código de barras (EAN16) já existe na base de dados.", "CodigoEan");
                }
                else
                {
                    await _IProduto.Add(Objeto);
                }
            }
        }

        public async Task Atualizar(Produto Objeto)
        {
            var validaDescricao = Objeto.ValidarPropriedadeString(Objeto.Descricao, "Descricao");
            if (validaDescricao)
            {
                var lstProdutos = await _IProduto.ListarProdutos(p => p.CodigoEan == Objeto.CodigoEan && p.Id != Objeto.Id);

                if (lstProdutos.Count > 0)
                {
                    Objeto.AdicionarNotificacao("O código de barras (EAN16) já existe na base de dados.", "CodigoEan");
                }
                else
                {
                    await _IProduto.Update(Objeto);
                }
            }
        }

        public async Task Excluir(Produto Objeto)
        {
            var validaDescricao = Objeto.ValidarPropriedadeString(Objeto.Descricao, "Descricao");
            if (validaDescricao)
            {
                var lstProdutos = await _IProduto.ListarProdutos(p => p.CodigoEan == Objeto.CodigoEan);

                if (lstProdutos == null || lstProdutos.Count == 0)
                {
                    Objeto.AdicionarNotificacao("O código de barras (EAN16) já existe na base de dados.", "CodigoEan");
                }
                else
                {
                    await _IProduto.Update(Objeto);
                }
            }
        }
    }
}
