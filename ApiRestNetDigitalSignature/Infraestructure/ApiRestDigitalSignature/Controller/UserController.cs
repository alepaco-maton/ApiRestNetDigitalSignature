using ApiRestNetDigitalSignature.Application.Service;
using ApiRestNetDigitalSignature.Dominio.Model;
using ApiRestNetDigitalSignature.Dominio.Port;
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
    private readonly IAppLogger _logger;
    private readonly IMultiLanguageMessagesService _messagesService;
    private readonly IDsUserRepository _repository;
    //private readonly IDocumentRepository _documentRepository;

    private CreateDsUserService _createUserService;
    /* private ReadUserUseCase _readUserUseCase;
     private UpdateUserUseCase _updateUserUseCase;
     private DeleteUserUseCase _deleteUserUseCase;
 */
    public UserController(IConfiguration configuration, IAppLogger logger,
                         IMultiLanguageMessagesService messagesService,
                         IDsUserRepository dsUserRepository,
                         CreateDsUserService createUserService
                             /*,
                              IDocumentRepository documentRepository*/)
    {
        _configuration = configuration;
        _logger = logger;
        _messagesService = messagesService;
        _repository = dsUserRepository;
        _createUserService = createUserService;
        //_documentRepository = documentRepository; 

        /*_readUserUseCase = new ReadUserUseCase(_userRepository);
        _updateUserUseCase = new UpdateUserUseCase(_messagesService, _userRepository, new UpdateUserValidator(_userRepository));
        _deleteUserUseCase = new DeleteUserUseCase(_messagesService, _userRepository, new DeleteUserValidator(_userRepository), _documentRepository);
        */

    }


    [HttpPost]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiDigitalSignatureExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiDigitalSignatureExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = new DsUser(request.UserName);
        user = await _createUserService.Create(user,
            _configuration.GetValue<string>("pathFolderByUser")
            );
        var response = new CreateUserResponse(user.Id, user.UserName);
        return Created($"/{response.Id}", response);

    }

}