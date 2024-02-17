using Entities.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApiGestaoProdutos.Common;
using TestApiGestaoProdutos.DTO;
using TestApiGestaoProdutos.Model;

namespace TestApiGestaoProdutos.Test
{
    internal class UT_Produto
    {
        [TestMethod]
        public void A_TestAdd()
        {
            DTOProduto produto = new DTOProduto()
            {
                Id = 0,
                Descricao = "Produto Teste Unitario " + DateTime.Now.ToString(),
                CodigoEan = "1" + DateTime.Now.ToString("ddMMyyhhmmss"),
                Valor = 29.90m,
                CategoriaProdutoId = 1
            };

            Util util = new Util();
            var result = util.ChamaApiPost("/Produto", produto).Result;

            var listaProduto = JsonConvert.DeserializeObject<ResponseMV<DTOProduto>>(result);

            Assert.IsTrue(listaProduto.Data.Any());
        }

        [TestMethod]
        public void B_TestGetAll()
        {
            AdicionarProduto();

            Util util = new Util();
            var result = util.ChamaApiGet("/Produto").Result;

            var listaProduto = JsonConvert.DeserializeObject<ResponseMV<DTOProduto>>(result);

            Assert.IsTrue(listaProduto.Data.Any());
        }

        [TestMethod]
        public void C_TestGetById()
        {
            var produto = AdicionarProduto();

            Util util = new Util();
            var result = util.ChamaApiGet("/Produto/" + produto.Id).Result;

            var listaProduto = JsonConvert.DeserializeObject<ResponseMV<DTOProduto>>(result);

            Assert.IsTrue(listaProduto.Data.Any());
        }

        [TestMethod]
        public void D_TestUpdate()
        {
            var produto = AdicionarProduto();
            produto.Descricao = "Produto Teste Unitario Alterado";

            Util util = new Util();
            var result = util.ChamaApiPut("/Produto", produto).Result;

            var listaProduto = JsonConvert.DeserializeObject<ResponseMV<DTOProduto>>(result);

            Assert.IsTrue(listaProduto.Data.Any());
        }

        [TestMethod]
        public void E_TestDelete()
        {
            var produto = AdicionarProduto();
            Util util = new Util();

            var result = util.ChamaApiGet("/Produto/" + produto.Id).Result;

            var listaProduto = JsonConvert.DeserializeObject<ResponseMV<DTOProduto>>(result);

            Assert.IsTrue(listaProduto.Data.Any());
        }

        public DTOProduto AdicionarProduto()
        {
            UT_CategoriaProduto utCategoriaProduto = new UT_CategoriaProduto();
            var categoriaProduto = utCategoriaProduto.AdicionarCategoriaProduto();

            DTOProduto produto = new DTOProduto()
            {
                Id = 0,
                Descricao = "Produto Teste Unitario " + DateTime.Now.ToString(),
                CodigoEan = "1" + DateTime.Now.ToString("ddMMyyhhmmss"),
                Valor = 29.90m,
                CategoriaProdutoId = categoriaProduto.Id
            };

            Util util = new Util();
            var result = util.ChamaApiPost("/Produto", produto).Result;

            var retorno = JsonConvert.DeserializeObject<ResponseMV<DTOProduto>>(result);

            return retorno.Data[0];
        }
    }
}
