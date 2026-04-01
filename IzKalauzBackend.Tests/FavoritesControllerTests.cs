using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using Xunit;
using IzKalauzBackend.Controllers;
using IzKalauzBackend.Models;
using IzKalauzBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace IzKalauzBackend.Tests.Controllers
{
    /// <summary>
    /// Unit tesztek a FavoritesController-hez
    /// </summary>
    public class FavoritesControllerTests
    {
        private readonly FavoritesController _controller;
        private readonly TestAppDbContext _context;
        private readonly Guid _userId;

        public FavoritesControllerTests()
        {
            // InMemory adatbázis beállítása minden teszthez
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"FavoritesTest_{Guid.NewGuid()}")
                .Options;

            _context = new TestAppDbContext(options);
            _controller = new FavoritesController(_context);

            _userId = Guid.NewGuid();
            SetupValidUser();
        }

        /// <summary>
        /// Beállítja a bejelentkezett felhasználót a teszthez
        /// </summary>
        private void SetupValidUser()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _userId.ToString())
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetFavorites_ReturnsUnauthorized_WhenUserNotLoggedIn()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.GetFavorites();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetFavoriteIds_ReturnsOkResult()
        {
            // Act
            var result = await _controller.GetFavoriteIds();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddFavorite_ReturnsNotFound_WhenRecipeDoesNotExist()
        {
            // Act
            var result = await _controller.AddFavorite(Guid.NewGuid());

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("Recept nem található", notFoundResult.Value?.ToString() ?? "");
        }

        [Fact]
        public async Task AddFavorite_ReturnsOk_WhenRecipeSuccessfullyAdded()
        {
            // Arrange
            var recipeId = Guid.NewGuid();
            var recipe = new Recipe
            {
                Id = recipeId,
                Title = "Teszt Recept"
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.AddFavorite(recipeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("hozzáadva", okResult.Value?.ToString() ?? "");
        }

        [Fact]
        public async Task AddFavorite_ReturnsConflict_WhenRecipeAlreadyInFavorites()
        {
            // Arrange
            var recipeId = Guid.NewGuid();

            // Recept létrehozása
            var recipe = new Recipe { Id = recipeId, Title = "Létező Recept" };
            _context.Recipes.Add(recipe);

            // Kedvenc létrehozása
            var favorite = new FavoriteRecipe
            {
                Id = Guid.NewGuid(),
                UserId = _userId,
                RecipeId = recipeId
            };

            _context.FavoriteRecipes.Add(favorite);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.AddFavorite(recipeId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Contains("már a kedvenceid között van", conflictResult.Value?.ToString() ?? "");
        }
    }

    // Teszt célú DbContext
    public class TestAppDbContext : AppDbContext
    {
        public TestAppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}