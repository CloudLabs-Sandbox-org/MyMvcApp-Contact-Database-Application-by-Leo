using MyMvcApp.Controllers;
using MyMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Linq;

namespace MyMvcApp.Tests
{
    public class UserControllerTests
    {
        public UserControllerTests()
        {
            // Antes de cada prueba, limpia o reestablece la lista estática.
            UserController.userlist.Clear();
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfUsers()
        {
            // Arrange
            var controller = new UserController();
            UserController.userlist.Add(new User { Id = 1, Name = "Test", Email = "test@example.com" });

            // Act
            var result = controller.Index(null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.IsType<System.Collections.Generic.List<User>>(result.Model);
            var model = result.Model as System.Collections.Generic.List<User>;
            Assert.Single(model); // Debe haber un usuario en la lista.
        }

        [Fact]
        public void Details_UserExists_ReturnsViewResult_WithUser()
        {
            // Arrange
            var controller = new UserController();
            UserController.userlist.Add(new User { Id = 2, Name = "Jane", Email = "jane@example.com" });

            // Act
            var result = controller.Details(2);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var user = Assert.IsType<User>(viewResult.Model);
            Assert.Equal(2, user.Id);
        }

        [Fact]
        public void Details_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_ValidUser_RedirectsToIndex()
        {
            // Arrange
            var controller = new UserController();
            var newUser = new User { Name = "John", Email = "john@example.com" };

            // Act
            var result = controller.Create(newUser);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(UserController.userlist);
        }

        [Fact]
        public void Create_InvalidUser_ReturnsViewWithModel()
        {
            // Arrange
            var controller = new UserController();
            // Forzamos un error de validación
            controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = controller.Create(new User());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<User>(viewResult.Model); // Retorna la vista con el modelo inválido
        }

        [Fact]
        public void Edit_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Edit(999, new User { Name = "DoesNotExist" });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_ValidUser_UpdatesAndRedirects()
        {
            // Arrange
            var controller = new UserController();
            UserController.userlist.Add(new User { Id = 1, Name = "OldName", Email = "old@example.com" });
            var updatedUser = new User { Id = 1, Name = "NewName", Email = "new@example.com" };

            // Act
            var result = controller.Edit(1, updatedUser);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            var user = UserController.userlist.FirstOrDefault(u => u.Id == 1);
            Assert.NotNull(user);
            Assert.Equal("NewName", user.Name);
        }
    }
}