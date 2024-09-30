using ApiRestNetDigitalSignature.Application.Port;
using ApiRestNetDigitalSignature.Dominio.Model;
using ApiRestNetDigitalSignature.Dominio.Port;
using ApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature.Controller;
using ApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature.Dto.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace TestApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature;

public class UserControllerTest
{

    private readonly Mock<IConfiguration> _configuration;
    private readonly Mock<ICreateDsUserService> _service;
    private readonly UserController _controller;

    public UserControllerTest()
    {
        _configuration = new Mock<IConfiguration>();
        _service = new Mock<ICreateDsUserService>();
        _controller = new UserController(_configuration.Object, _service.Object);
    }

    [Fact]
    public async Task createSuccessful()
    {
        // Arrange
        const string userNameTest = "testuser";
        var request = new CreateUserRequest(userNameTest);
        var createdUser = new DsUser(request.UserName) { Id = 1 };

        _configuration.Setup(config => config["pathFolderByUser"]).Returns("some/folder/path");

        _service.Setup(service => service.Create(It.IsAny<DsUser>(), _configuration.Object["pathFolderByUser"]))
            .ReturnsAsync(createdUser);

        // Act
        var result = await _controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.NotNull(((CreateUserResponse)createdResult.Value).Id); // Verify user ID in response
        Assert.Equal(userNameTest, ((CreateUserResponse)createdResult.Value).UserName); // Verify username in response

    }

}