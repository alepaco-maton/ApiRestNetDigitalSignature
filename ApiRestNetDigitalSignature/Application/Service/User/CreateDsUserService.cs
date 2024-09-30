using ApiRestNetDigitalSignature.Application.Port;
using ApiRestNetDigitalSignature.Application.Service.User.Validator;
using ApiRestNetDigitalSignature.Dominio.Common;
using ApiRestNetDigitalSignature.Dominio.Model;
using ApiRestNetDigitalSignature.Dominio.Port;

namespace ApiRestNetDigitalSignature.Application.Service;

public class CreateDsUserService : ICreateDsUserService
{
    private IAppLogger _logger;
    private IMultiLanguageMessagesService _mlms;

    private readonly IUnitOfWork _unitOfWork;

    private IValidator<DsUser> validator;

    private ICreateCertAndPairKeyUseCase createCertAndPairKeyUseCase;

    public CreateDsUserService(IAppLogger log, IMultiLanguageMessagesService messagesService,
    IUnitOfWork unitOfWork,
    ICreateCertAndPairKeyUseCase certAndPairKeyUseCase)
    {
        _logger = log;
        _mlms = messagesService;
        _unitOfWork = unitOfWork;
        validator = new CreateDsUserValidator(_unitOfWork.DsUserRepository);
        createCertAndPairKeyUseCase = certAndPairKeyUseCase;
    }

    public async Task<DsUser> Create(DsUser model, string pathFolderByUser)
    {
        string errorCode = validator.Validate(model);

        if (!AppTools.IsSuccessfulErrorCode(errorCode))
        {
            throw new DigitalSignatureException(errorCode,
                    _mlms.GetMessage(errorCode));
        }

        try
        {
            _unitOfWork.BeginTransaction();

            model = await _unitOfWork.DsUserRepository.AddAsync(model);
            model = createCertAndPairKeyUseCase.Create(model, pathFolderByUser);
            model = await _unitOfWork.DsUserRepository.UpdateAsync(model);

            _unitOfWork.CommitTransaction();

            return model;
        }
        catch (Exception e)
        {
            _logger.Error(e.Message, e);

            _unitOfWork.RollbackTransaction();

            throw new DigitalSignatureException(ErrorCode.ERROR_PROCESSING_THE_TRANSACTION,
                    _mlms.GetMessage(ErrorCode.ERROR_PROCESSING_THE_TRANSACTION), e);
        }
    }

}