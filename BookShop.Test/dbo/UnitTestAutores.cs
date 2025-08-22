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
    public class UnitTestAutores
    {
        private readonly IAutoresRepository _autoresRepository;

        public UnitTestAutores()
        {
            var options = new DbContextOptionsBuilder<BookShopContext>()
                .UseInMemoryDatabase(databaseName: "BookShopDB")
                .Options;

            var context = new BookShopContext(options);

            var mockLogger = new Mock<ILogger<AutoresRepository>>();
            var validateAutores = new AutoresValidation();

            _autoresRepository = new AutoresRepository(
                context,
                mockLogger.Object,
                validateAutores
            );
        }

        [Fact]
        public async Task SaveAutores_ShouldReturnFailure_WhenNombreIsNullOrEmpty()
        {
            var autor = new Autores
            {
                Nombre = null,
                Apellido = "Perez",
                Pais = "RD",
                FechaNacimiento = new DateTime(1980, 1, 1)
            };

            var result = await _autoresRepository.Save(autor);

            Assert.False(result.Success);
            Assert.Equal("El nombre es requerido y debe tener máximo 100 caracteres.", result.Message);
        }

        [Fact]
        public async Task SaveAutores_ShouldReturnFailure_WhenApellidoIsNullOrEmpty()
        {
            var autor = new Autores
            {
                Nombre = "Juan",
                Apellido = "",
                Pais = "RD",
                FechaNacimiento = new DateTime(1980, 1, 1)
            };

            var result = await _autoresRepository.Save(autor);

            Assert.False(result.Success);
            Assert.Equal("El apellido es requerido y debe tener máximo 100 caracteres.", result.Message);
        }

        [Fact]
        public async Task SaveAutores_ShouldReturnFailure_WhenPaisExceedsMaxLength()
        {
            var autor = new Autores
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Pais = new string('A', 101),
                FechaNacimiento = new DateTime(1980, 1, 1)
            };

            var result = await _autoresRepository.Save(autor);

            Assert.False(result.Success);
            Assert.Equal("El país debe tener máximo 100 caracteres.", result.Message);
        }

        [Fact]
        public async Task SaveAutores_ShouldReturnFailure_WhenFechaNacimientoIsInFuture()
        {
            var autor = new Autores
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Pais = "RD",
                FechaNacimiento = DateTime.Now.AddDays(1) // Fecha futura
            };

            var result = await _autoresRepository.Save(autor);

            Assert.False(result.Success);
            Assert.Equal("La fecha de nacimiento no puede ser en el futuro.", result.Message);
        }

        [Fact]
        public async Task UpdateAutores_ShouldReturnFailure_WhenAutorIDIsInvalid()
        {
            var autor = new Autores
            {
                AutorID = 0, // ID inválido
                Nombre = "Juan",
                Apellido = "Perez",
                Pais = "RD",
                FechaNacimiento = new DateTime(1980, 1, 1)
            };

            var result = await _autoresRepository.Update(autor);

            Assert.False(result.Success);
            Assert.Equal("El ID del autor es requerido.", result.Message);
        }

        [Fact]
        public async Task RemoveAutores_ShouldReturnFailure_WhenAutorIDIsInvalid()
        {
            var autor = new Autores
            {
                AutorID = 0
            };

            var result = await _autoresRepository.Remove(autor);

            Assert.False(result.Success);
            Assert.Equal("El autor es requerido.", result.Message);
        }
    }
}
