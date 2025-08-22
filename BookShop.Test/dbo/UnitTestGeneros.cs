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
    public class UnitTestGeneros
    {
        private readonly IGenerosRepository _generosRepository;

        public UnitTestGeneros()
        {
            var options = new DbContextOptionsBuilder<BookShopContext>()
                .UseInMemoryDatabase(databaseName: "BookShopDB")
                .Options;

            var context = new BookShopContext(options);

            var mockLogger = new Mock<ILogger<GenerosRepository>>();
            var validateGeneros = new GenerosValidation();

            _generosRepository = new GenerosRepository(
                context,
                mockLogger.Object,
                validateGeneros
            );
        }

        [Fact]
        public async Task SaveGeneros_ShouldReturnFailure_WhenNombreIsNullOrEmpty()
        {
            var genero = new Generos
            {
                Nombre = null
            };

            var result = await _generosRepository.Save(genero);

            Assert.False(result.Success);
            Assert.Equal("El nombre del género es requerido y debe tener máximo 100 caracteres.", result.Message);
        }

        [Fact]
        public async Task UpdateGeneros_ShouldReturnFailure_WhenGeneroIDIsInvalid()
        {
            var genero = new Generos
            {
                GeneroID = 0,
                Nombre = "Ficción"
            };

            var result = await _generosRepository.Update(genero);

            Assert.False(result.Success);
            Assert.Equal("El ID del género es requerido.", result.Message);
        }

        [Fact]
        public async Task UpdateGeneros_ShouldReturnFailure_WhenNombreIsNullOrEmpty()
        {
            var genero = new Generos
            {
                GeneroID = 1,
                Nombre = ""
            };

            var result = await _generosRepository.Update(genero);

            Assert.False(result.Success);
            Assert.Equal("El nombre del género es requerido y debe tener máximo 100 caracteres.", result.Message);
        }

        [Fact]
        public async Task RemoveGeneros_ShouldReturnFailure_WhenGeneroIDIsInvalid()
        {
            var genero = new Generos
            {
                GeneroID = 0
            };

            var result = await _generosRepository.Remove(genero);

            Assert.False(result.Success);
            Assert.Equal("El género es requerido.", result.Message);
        }
    }
}
