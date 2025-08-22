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
    public class UnitTestDetallePedidos
    {
        private readonly IDetallePedidosRepository _detallePedidosRepository;

        public UnitTestDetallePedidos()
        {
            var options = new DbContextOptionsBuilder<BookShopContext>()
                .UseInMemoryDatabase(databaseName: "BookShopDB")
                .Options;

            var context = new BookShopContext(options);

            var mockLogger = new Mock<ILogger<DetallePedidosRepository>>();
            var validateDetallePedidos = new DetallePedidosValidation();

            _detallePedidosRepository = new DetallePedidosRepository(
                context,
                mockLogger.Object,
                validateDetallePedidos
            );
        }

        [Fact]
        public async Task SaveDetallePedidos_ShouldReturnFailure_WhenPedidoIDIsZeroOrLess()
        {
            var detalle = new DetallePedidos
            {
                PedidoID = 0,
                LibroID = 1,
                Cantidad = 1,
                PrecioUnitario = 10
            };

            var result = await _detallePedidosRepository.Save(detalle);

            Assert.False(result.Success);
            Assert.Equal("El ID del pedido es requerido.", result.Message);
        }

        [Fact]
        public async Task SaveDetallePedidos_ShouldReturnFailure_WhenLibroIDIsZeroOrLess()
        {
            var detalle = new DetallePedidos
            {
                PedidoID = 1,
                LibroID = 0,
                Cantidad = 1,
                PrecioUnitario = 10
            };

            var result = await _detallePedidosRepository.Save(detalle);

            Assert.False(result.Success);
            Assert.Equal("El ID del libro es requerido.", result.Message);
        }

        [Fact]
        public async Task SaveDetallePedidos_ShouldReturnFailure_WhenCantidadIsZeroOrLess()
        {
            var detalle = new DetallePedidos
            {
                PedidoID = 1,
                LibroID = 1,
                Cantidad = 0,
                PrecioUnitario = 10
            };

            var result = await _detallePedidosRepository.Save(detalle);

            Assert.False(result.Success);
            Assert.Equal("La cantidad debe ser mayor a 0.", result.Message);
        }

        [Fact]
        public async Task SaveDetallePedidos_ShouldReturnFailure_WhenPrecioUnitarioIsZeroOrLess()
        {
            var detalle = new DetallePedidos
            {
                PedidoID = 1,
                LibroID = 1,
                Cantidad = 1,
                PrecioUnitario = 0
            };

            var result = await _detallePedidosRepository.Save(detalle);

            Assert.False(result.Success);
            Assert.Equal("El precio unitario debe ser mayor a 0.", result.Message);
        }

        [Fact]
        public async Task UpdateDetallePedidos_ShouldReturnFailure_WhenDetalleIDIsInvalid()
        {
            var detalle = new DetallePedidos
            {
                DetalleID = 0,
                PedidoID = 1,
                LibroID = 1,
                Cantidad = 1,
                PrecioUnitario = 10
            };

            var result = await _detallePedidosRepository.Update(detalle);

            Assert.False(result.Success);
            Assert.Equal("El ID del detalle del pedido es requerido.", result.Message);
        }

        [Fact]
        public async Task RemoveDetallePedidos_ShouldReturnFailure_WhenDetalleIDIsInvalid()
        {
            var detalle = new DetallePedidos
            {
                DetalleID = 0
            };

            var result = await _detallePedidosRepository.Remove(detalle);

            Assert.False(result.Success);
            Assert.Equal("El detalle del pedido es requerido.", result.Message);
        }
    }
}
