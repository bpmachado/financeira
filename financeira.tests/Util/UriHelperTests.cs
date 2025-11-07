using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using financeira.Util;
using Microsoft.AspNetCore.Http;

namespace financeiro.tests.Util
{
    public class UriHelperTests
    {
        [Fact(DisplayName = "Deve gerar URI correta com HTTP padrão")]
        public void GerarHeaderLocation_DeveGerarUriHttpCorreta()
        {
            var id = Guid.NewGuid();
            var context = new DefaultHttpContext();
            context.Request.Scheme = "http";
            context.Request.Host = new HostString("localhost", 8080);
            context.Request.Path = "/api/contratos";

            var result = UriHelper.GerarHeaderLocation(context.Request, id);

            Assert.Equal($"http://localhost:8080/api/contratos/{id}", result.ToString());
        }


        [Fact(DisplayName = "Deve usar porta do Host quando especificada")]
        public void GerarHeaderLocation_DeveUsarPortaEspecifica()
        {
            var id = Guid.NewGuid();
            var context = new DefaultHttpContext();
            context.Request.Scheme = "http";
            context.Request.Host = new HostString("myapi.com", 8081);
            context.Request.Path = "/api/teste";

            var result = UriHelper.GerarHeaderLocation(context.Request, id);

            Assert.Equal($"http://myapi.com:8081/api/teste/{id}", result.ToString());
        }
    }
}
