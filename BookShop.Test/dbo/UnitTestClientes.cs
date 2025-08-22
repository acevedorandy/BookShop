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
    public class UnitTestClientes
    {
        private readonly IClientesRepository _clientesRepository;

        public UnitTestClientes()
        {
            var options = new DbContextOptionsBuilder<BookShopContext>()
                .UseInMemoryDatabase(databaseName: "BookShopDB")
                .Options;

            var context = new BookShopContext(options);

            var mockLogger = new Mock<ILogger<ClientesRepository>>();
            var validateClientes = new ClientesValidation();

            _clientesRepository = new ClientesRepository(
                context,
                mockLogger.Object,
                validateClientes
            );
        }

        [Fact]
        public async Task SaveClientes_ShouldReturnFailure_WhenNombreIsNullOrEmpty()
        {
            var cliente = new Clientes
            {
                Nombre = null,
                Apellido = "Perez",
                Correo = "correo@example.com"
            };

            var result = await _clientesRepository.Save(cliente);

            Assert.False(result.Success);
            Assert.Equal("El nombre es requerido y debe tener máximo 100 caracteres.", result.Message);
        }

        [Fact]
        public async Task SaveClientes_ShouldReturnFailure_WhenApellidoIsNullOrEmpty()
        {
            var cliente = new Clientes
            {
                Nombre = "Juan",
                Apellido = "",
                Correo = "correo@example.com"
            };

            var result = await _clientesRepository.Save(cliente);

            Assert.False(result.Success);
            Assert.Equal("El apellido es requerido y debe tener máximo 100 caracteres.", result.Message);
        }

        [Fact]
        public async Task SaveClientes_ShouldReturnFailure_WhenCorreoIsNullOrEmpty()
        {
            var cliente = new Clientes
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Correo = ""
            };

            var result = await _clientesRepository.Save(cliente);

            Assert.False(result.Success);
            Assert.Equal("El correo es requerido y debe tener máximo 150 caracteres.", result.Message);
        }

        [Fact]
        public async Task SaveClientes_ShouldReturnFailure_WhenTelefonoExceedsMaxLength()
        {
            var cliente = new Clientes
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Correo = "correo@example.com",
                Telefono = new string('1', 21) // 21 caracteres
            };

            var result = await _clientesRepository.Save(cliente);

            Assert.False(result.Success);
            Assert.Equal("El teléfono debe tener máximo 20 caracteres.", result.Message);
        }

        [Fact]
        public async Task SaveClientes_ShouldReturnFailure_WhenDireccionExceedsMaxLength()
        {
            var cliente = new Clientes
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Correo = "correo@example.com",
                Direccion = new string('A', 251) // 251 caracteres
            };

            var result = await _clientesRepository.Save(cliente);

            Assert.False(result.Success);
            Assert.Equal("La dirección debe tener máximo 250 caracteres.", result.Message);
        }

        [Fact]
        public async Task UpdateClientes_ShouldReturnFailure_WhenClienteIDIsInvalid()
        {
            var cliente = new Clientes
            {
                ClienteID = 0,
                Nombre = "Juan",
                Apellido = "Perez",
                Correo = "correo@example.com"
            };

            var result = await _clientesRepository.Update(cliente);

            Assert.False(result.Success);
            Assert.Equal("El ID del cliente es requerido.", result.Message);
        }

        [Fact]
        public async Task RemoveClientes_ShouldReturnFailure_WhenClienteIDIsInvalid()
        {
            var cliente = new Clientes
            {
                ClienteID = 0
            };

            var result = await _clientesRepository.Remove(cliente);

            Assert.False(result.Success);
            Assert.Equal("El cliente es requerido.", result.Message);
        }
    }
}
