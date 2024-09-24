using Moq;

namespace TestApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature;

public class UserControllerTest
{

    private readonly UserController _controller;
    private readonly Mock<UserService> _service;

    public UserControllerTest()
    {
        _service = new Mock<IUserService>();
        _controller = new UserController(_service.Object);
    }

    [Fact]
    public void createSuccessful()
    {
        CreateUserRequest dto = new CreateUserRequest();
        dto.setUserName("alepaco.maton");
        DsUser user = new DsUser(); 
       
        _service.Setup(s => s.create(dto)).ReturnsAsync(user);

        var result = await _controller.create(dto);

        
        var createdResult = Assert.IsType<CreateUserResponse>(result);
        var returnedUser = Assert.IsAssignableFrom<DsUser>(createdResult.Value);
        Assert.Equal(user, returnedUser);
     
    }

}