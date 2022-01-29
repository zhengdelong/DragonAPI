using Domain;
using DragonAPI.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dragon.API.Test
{
    public class UserControllerTests
    {
        private readonly Mock<IUserServices> userServices = new();
        [Fact]
        public async Task AddUser_WithAddFail_ReturnBadRequest()
        {
            //Arrange
            userServices.Setup(repo => repo.AddUser(It.IsAny<User>())).ReturnsAsync(false);

            var controller = new UserController(userServices.Object);
            //Act
            var res = await controller.AddUser(new User());
            //Assert
            res.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void GetUser_WithExistingUser_ReturnExpectedUser()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new User() { UserID = userId };
            userServices.Setup(repo => repo.User(userId)).Returns(user);
            var controller = new UserController(userServices.Object);
            //Act
            var res = controller.GetUser(userId);
            //Assert
            res.Should().BeEquivalentTo(user, option => option.ComparingByMembers<User>());
        }
    }
}
