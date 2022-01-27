using Domain;
using DragonAPI.Controllers;
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
        [Fact]
        public async Task AddUser_WithAddFail_ReturnBadRequest()
        {
            //Arrange
            var userServices = new Mock<IUserServices>();
          
            userServices.Setup(repo => repo.AddUser(It.IsAny<User>())).ReturnsAsync(false);

            var controller = new UserController(userServices.Object);
            //Act
            var res = await controller.AddUser(new User());
            //Assert
            Assert.IsType<BadRequestObjectResult>(res);
        }
    }
}
