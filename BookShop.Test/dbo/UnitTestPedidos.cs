using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Context;
using BookShop.Persistance.Interfaces.dbo;
using BookShop.Persistance.Repositories.dbo;
using BookShop.Persistance.Validations.dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookShop.Test.dbo
{
    public class UnitTestPedidos
    {
        private readonly IPedidosRepository _pedidosRepository;

        public UnitTestPedidos()
        {
            var options = new DbContextOptionsBuilder<BookShopContext>()
                .UseInMemoryDatabase(databaseName: "BookShopDB")
                .Options;

            var context = new BookShopContext(options);

            var mockLogger = new Mock<ILogger<PedidosRepository>>();
            var validatePedidos = new PedidosValidation();

            _pedidosRepository = new PedidosRepository(
                context,
                mockLogger.Object,
                validatePedidos
            );
        }

        [Fact]
        public async Task SavePedidos_ShouldReturnFailure_WhenClienteIDIsZeroOrLess()
        {
            var pedido = new Pedidos
            {
                ClienteID = 0,
                MontoTotal = 100
            };

            var result = await _pedidosRepository.Save(pedido);

            Assert.False(result.Success);
            Assert.Equal("El ID del cliente es requerido.", result.Message);
        }

        [Fact]
        public async Task SavePedidos_ShouldReturnFailure_WhenMontoTotalIsNegative()
        {
            var pedido = new Pedidos
            {
                ClienteID = 1,
                MontoTotal = -10
            };

            var result = await _pedidosRepository.Save(pedido);

            Assert.False(result.Success);
            Assert.Equal("El monto total no puede ser negativo.", result.Message);
        }

        [Fact]
        public async Task UpdatePedidos_ShouldReturnFailure_WhenPedidoIDIsInvalid()
        {
            var pedido = new Pedidos
            {
                PedidoID = 0,
                ClienteID = 1,
                MontoTotal = 100
            };

            var result = await _pedidosRepository.Update(pedido);

            Assert.False(result.Success);
            Assert.Equal("El ID del pedido es requerido.", result.Message);
        }

        [Fact]
        public async Task UpdatePedidos_ShouldReturnFailure_WhenClienteIDIsZeroOrLess()
        {
            var pedido = new Pedidos
            {
                PedidoID = 1,
                ClienteID = 0,
                MontoTotal = 100
            };

            var result = await _pedidosRepository.Update(pedido);

            Assert.False(result.Success);
            Assert.Equal("El ID del cliente es requerido.", result.Message);
        }

        [Fact]
        public async Task UpdatePedidos_ShouldReturnFailure_WhenMontoTotalIsNegative()
        {
            var pedido = new Pedidos
            {
                PedidoID = 1,
                ClienteID = 1,
                MontoTotal = -50
            };

            var result = await _pedidosRepository.Update(pedido);

            Assert.False(result.Success);
            Assert.Equal("El monto total no puede ser negativo.", result.Message);
        }

        [Fact]
        public async Task RemovePedidos_ShouldReturnFailure_WhenPedidoIDIsInvalid()
        {
            var pedido = new Pedidos
            {
                PedidoID = 0
            };

            var result = await _pedidosRepository.Remove(pedido);

            Assert.False(result.Success);
            Assert.Equal("El pedido es requerido.", result.Message);
        }
    }
}
