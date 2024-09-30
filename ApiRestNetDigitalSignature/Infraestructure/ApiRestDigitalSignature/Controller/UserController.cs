using ApiRestNetDigitalSignature.Application.Port;
using ApiRestNetDigitalSignature.Application.Service;
using ApiRestNetDigitalSignature.Dominio.Model;
using ApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature.Dto.User;
using ApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature.Controller;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "v1")]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private ICreateDsUserService _createUserService;

    public UserController(IConfiguration configuration, ICreateDsUserService createUserService)
    {
        _configuration = configuration;
        _createUserService = createUserService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiDigitalSignatureExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiDigitalSignatureExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = new DsUser(request.UserName);
        user = await _createUserService.Create(user,
        _configuration["pathFolderByUser"]
        );
        var response = new CreateUserResponse(user.Id, user.UserName);
        return Created($"/{response.Id}", response);

    }

}