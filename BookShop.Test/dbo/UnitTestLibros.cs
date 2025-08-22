using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Context;
using BookShop.Persistance.Interfaces.dbo;
using BookShop.Persistance.Repositories.dbo;
using BookShop.Persistance.Validations.dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BookShop.Test.dbo
{
    public class UnitTestLibros
    {
        private readonly ILibrosRepository _librosRepository;

        public UnitTestLibros()
        {
            var options = new DbContextOptionsBuilder<BookShopContext>()
                .UseInMemoryDatabase(databaseName: "BookShopDB")
                .Options;

            var context = new BookShopContext(options);

            var mockLogger = new Mock<ILogger<LibrosRepository>>();
            var validateLibros = new LibrosValidation();

            _librosRepository = new LibrosRepository(
                context,
                mockLogger.Object,
                validateLibros
            );
        }

        [Fact]
        public async Task SaveLibros_ShouldReturnFailure_WhenTituloIsNullOrEmpty()
        {
            var libro = new Libros
            {
                Titulo = null,
                ISBN = "1234567890",
                AñoPublicacion = 2020,
                Precio = 10,
                Stock = 5,
                AutorID = 1,
                GeneroID = 1
            };

            var result = await _librosRepository.Save(libro);

            Assert.False(result.Success);
            Assert.Equal("El título es requerido y debe tener máximo 200 caracteres.", result.Message);
        }

        [Fact]
        public async Task SaveLibros_ShouldReturnFailure_WhenISBNIsNullOrEmpty()
        {
            var libro = new Libros
            {
                Titulo = "Libro de prueba",
                ISBN = "",
                AñoPublicacion = 2020,
                Precio = 10,
                Stock = 5,
                AutorID = 1,
                GeneroID = 1
            };

            var result = await _librosRepository.Save(libro);

            Assert.False(result.Success);
            Assert.Equal("El ISBN es requerido y debe tener máximo 20 caracteres.", result.Message);
        }

        [Fact]
        public async Task SaveLibros_ShouldReturnFailure_WhenAñoPublicacionIsInvalid()
        {
            var libro = new Libros
            {
                Titulo = "Libro de prueba",
                ISBN = "1234567890",
                AñoPublicacion = DateTime.Now.Year + 1,
                Precio = 10,
                Stock = 5,
                AutorID = 1,
                GeneroID = 1
            };

            var result = await _librosRepository.Save(libro);

            Assert.False(result.Success);
            Assert.Equal("El año de publicación no es válido.", result.Message);
        }

        [Fact]
        public async Task SaveLibros_ShouldReturnFailure_WhenPrecioIsZeroOrLess()
        {
            var libro = new Libros
            {
                Titulo = "Libro de prueba",
                ISBN = "1234567890",
                AñoPublicacion = 2020,
                Precio = 0,
                Stock = 5,
                AutorID = 1,
                GeneroID = 1
            };

            var result = await _librosRepository.Save(libro);

            Assert.False(result.Success);
            Assert.Equal("El precio debe ser mayor a 0.", result.Message);
        }

        [Fact]
        public async Task SaveLibros_ShouldReturnFailure_WhenStockIsNegative()
        {
            var libro = new Libros
            {
                Titulo = "Libro de prueba",
                ISBN = "1234567890",
                AñoPublicacion = 2020,
                Precio = 10,
                Stock = -1,
                AutorID = 1,
                GeneroID = 1
            };

            var result = await _librosRepository.Save(libro);

            Assert.False(result.Success);
            Assert.Equal("El stock no puede ser negativo.", result.Message);
        }

        [Fact]
        public async Task SaveLibros_ShouldReturnFailure_WhenAutorIDIsZeroOrLess()
        {
            var libro = new Libros
            {
                Titulo = "Libro de prueba",
                ISBN = "1234567890",
                AñoPublicacion = 2020,
                Precio = 10,
                Stock = 5,
                AutorID = 0,
                GeneroID = 1
            };

            var result = await _librosRepository.Save(libro);

            Assert.False(result.Success);
            Assert.Equal("El ID del autor es requerido.", result.Message);
        }

        [Fact]
        public async Task SaveLibros_ShouldReturnFailure_WhenGeneroIDIsZeroOrLess()
        {
            var libro = new Libros
            {
                Titulo = "Libro de prueba",
                ISBN = "1234567890",
                AñoPublicacion = 2020,
                Precio = 10,
                Stock = 5,
                AutorID = 1,
                GeneroID = 0
            };

            var result = await _librosRepository.Save(libro);

            Assert.False(result.Success);
            Assert.Equal("El ID del género es requerido.", result.Message);
        }

        [Fact]
        public async Task UpdateLibros_ShouldReturnFailure_WhenLibroIDIsInvalid()
        {
            var libro = new Libros
            {
                LibroID = 0,
                Titulo = "Libro de prueba",
                ISBN = "1234567890",
                AñoPublicacion = 2020,
                Precio = 10,
                Stock = 5,
                AutorID = 1,
                GeneroID = 1
            };

            var result = await _librosRepository.Update(libro);

            Assert.False(result.Success);
            Assert.Equal("El ID del libro es requerido.", result.Message);
        }

        [Fact]
        public async Task RemoveLibros_ShouldReturnFailure_WhenLibroIDIsInvalid()
        {
            var libro = new Libros
            {
                LibroID = 0
            };

            var result = await _librosRepository.Remove(libro);

            Assert.False(result.Success);
            Assert.Equal("El libro es requerido.", result.Message);
        }
    }
}
