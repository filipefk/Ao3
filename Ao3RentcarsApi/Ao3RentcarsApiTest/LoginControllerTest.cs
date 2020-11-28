using Ao3RentcarsApi.Controllers;
using Ao3RentcarsApi.Models;
using Ao3RentcarsApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ao3RentcarsApiTest
{
    public class LoginControllerTest
    {
        readonly LoginController _controller;
        readonly RentcarsContext _context;

        public LoginControllerTest()
        {
            DbContextOptions<RentcarsContext> options = new DbContextOptionsBuilder<RentcarsContext>()
                .UseSqlite(@"Data Source=..\..\..\..\Ao3RentcarsApi\Ao3Rentcars.db")
                .Options;

            _context = new RentcarsContext(options);
            
            _controller = new LoginController(_context);
        }

        [Fact]
        public void TestaLoginInvalido()
        {
            // Act
            LoginDto loginDto = new LoginDto()
            {
                Login = "Nome",
                Senha = "Senha"
            };

            var okResult = _controller.Login(loginDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(okResult.Result);
        }

        [Fact]
        public void TestaLoginValido()
        {
            // Act
            LoginDto loginDto = new LoginDto()
            {
                Login = "Admin",
                Senha = "SenhaAdmin"
            };

            var okResult = _controller.Login(loginDto);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
    }
}
