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
    public class ServiceCategoriaProduto : IServiceCategoriaProduto
    {
        private readonly ICategoriaProduto _ICategoriaProduto;

        public ServiceCategoriaProduto(ICategoriaProduto ICategoriaProduto)
        {
            _ICategoriaProduto = ICategoriaProduto;
        }

        public async Task Adicionar(CategoriaProduto Objeto)
        {
            await _ICategoriaProduto.Add(Objeto);
        }

        public async Task Atualizar(CategoriaProduto Objeto)
        {
            var catProd = await _ICategoriaProduto.GetEntityById(Objeto.Id);

            if (catProd == null)
            {
                Objeto.AdicionarNotificacao("Categoria não encontrada.", "Id");
            }
            else
            {
                await _ICategoriaProduto.Update(Objeto);
            }
        }

        public async Task Excluir(CategoriaProduto Objeto)
        {
            var catProd = await _ICategoriaProduto.GetEntityById(Objeto.Id);

            if (catProd == null)
            {
                Objeto.AdicionarNotificacao("Categoria não encontrada.", "Id");
            }
            else
            {
                await _ICategoriaProduto.Delete(Objeto);
            }
        }

        public async Task<CategoriaProduto> ObterPorId(int Id)
        {
            return await _ICategoriaProduto.ObterCategoriaPorIdComInclude(Id);
        }

    }
}
