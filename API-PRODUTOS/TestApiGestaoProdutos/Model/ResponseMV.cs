using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApiGestaoProdutos.Model
{
    public class ResponseMV<T> where T : class
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; }      
    }

    public class ResponseMV
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
