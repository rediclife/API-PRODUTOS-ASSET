using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Entities.Entities;
using TestApiGestaoProdutos.DTO;
using TestApiGestaoProdutos.Model;
using TestApiGestaoProdutos.Common;



namespace TestApiGestaoProdutos.Test
{
    [TestClass]
    public class UT_CategoriaProduto
    {
        [TestMethod]
        public void A_TestAdd()
        {
            DTOCategoriaProduto categoriaProduto = new DTOCategoriaProduto()
            {
                Id = 0,
                Descricao = "Categoria Teste Unitário",
                UserId = "5739c00e-64e3-48ee-85d9-be4d72720555"
            };

            Util util = new Util();
            var result = util.ChamaApiPost("/CategoriaProduto", categoriaProduto).Result;

            var listaCategoriaProduto = JsonConvert.DeserializeObject<ResponseMV<DTOCategoriaProduto>>(result);

            Assert.IsTrue(listaCategoriaProduto.Data.Any());
        }

        [TestMethod]
        public void B_TestGetAll()
        {
            AdicionarCategoriaProduto();
            Util util = new Util();
            var result = util.ChamaApiGet("/CategoriaProduto").Result;

            var listaCategoriaProduto = JsonConvert.DeserializeObject<ResponseMV<DTOCategoriaProduto>>(result);

            Assert.IsTrue(listaCategoriaProduto.Data.Any());
        }

        [TestMethod]
        public void C_TestGetById()
        {
            var categoriaProduto = AdicionarCategoriaProduto();

            Util util = new Util();
            var result = util.ChamaApiGet("/CategoriaProduto/" + categoriaProduto.Id).Result;

            var listaCategoriaProduto = JsonConvert.DeserializeObject<ResponseMV<DTOCategoriaProduto>>(result);

            Assert.IsTrue(listaCategoriaProduto.Data.Any());
        }

        [TestMethod]
        public void D_TestUpdate()
        {
            var categoriaProduto = AdicionarCategoriaProduto();
            categoriaProduto.Descricao = "Categoria Teste Unitário Editada";
            Util util = new Util();
            var result = util.ChamaApiPut("/CategoriaProduto", categoriaProduto).Result;

            var listaCategoriaProduto = JsonConvert.DeserializeObject<ResponseMV<DTOCategoriaProduto>>(result);

            Assert.IsTrue(listaCategoriaProduto.Data.Any());
        }

        [TestMethod]
        public void E_TestDelete()
        {
            var categoriaProduto = AdicionarCategoriaProduto();
            Util util = new Util();
            var result = util.ChamaApiGet("/CategoriaProduto/" + categoriaProduto.Id).Result;

            var listaCategoriaProduto = JsonConvert.DeserializeObject<ResponseMV<DTOCategoriaProduto>>(result);

            Assert.IsTrue(listaCategoriaProduto.Data.Any());
        }

        public DTOCategoriaProduto AdicionarCategoriaProduto()
        {
            DTOCategoriaProduto categoriaProduto = new DTOCategoriaProduto()
            {
                Id = 0,
                Descricao = "Categoria Teste Unitário " + DateTime.Now.ToString(),
                UserId = "5739c00e-64e3-48ee-85d9-be4d72720555"
            };

            Util util = new Util();
            var result = util.ChamaApiPost("/CategoriaProduto", categoriaProduto).Result;

            var retorno = JsonConvert.DeserializeObject<ResponseMV<DTOCategoriaProduto>>(result);

            return retorno.Data[0];
        }
    }
}