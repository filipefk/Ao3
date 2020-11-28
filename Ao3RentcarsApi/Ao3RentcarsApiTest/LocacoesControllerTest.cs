using Ao3RentcarsApi.Controllers;
using Ao3RentcarsApi.Models;
using Ao3RentcarsApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Ao3RentcarsApiTest
{
    public class LocacoesControllerTest
    {
        readonly LocacoesController _controller;
        readonly RentcarsContext _context;

        public LocacoesControllerTest()
        {
            DbContextOptions<RentcarsContext> options = new DbContextOptionsBuilder<RentcarsContext>()
                .UseSqlite(@"Data Source=..\..\..\..\Ao3RentcarsApi\Ao3Rentcars.db")
                .Options;

            _context = new RentcarsContext(options);

            _controller = new LocacoesController(_context);
        }

        [Fact]
        public void TestaListaTodos()
        {
            // Act
            var retorno = _controller.ListaTodos();

            // Assert
            Assert.IsType<ActionResult<System.Collections.Generic.IEnumerable<LocacaoDto>>>(retorno.Result);
        }

        [Fact]
        public void TestaBuscaNotFound()
        {
            // Act
            var retorno = _controller.Busca(0);

            // Assert
            Assert.IsType<NotFoundResult>(retorno.Result.Result);
        }

        [Fact]
        public void TestaAlteraNotFound()
        {
            // Act

            LocacaoDto locacaoDto = new LocacaoDto();

            var retorno = _controller.Altera(0, locacaoDto);

            // Assert
            Assert.IsType<NotFoundResult>(retorno.Result);
        }

        [Fact]
        public void TestaInsereInvalido()
        {
            // Act

            LocacaoDto locacaoDto = new LocacaoDto();

            var retorno = _controller.Insere(locacaoDto);

            // Assert
            Assert.IsType<ArgumentException>(retorno.Exception.InnerExceptions[0]);
        }

        [Fact]
        public void TestaExcluiNotFound()
        {
            // Act
            var retorno = _controller.Exclui(0);

            // Assert
            Assert.IsType<NotFoundResult>(retorno.Result);
        }
    }
}
