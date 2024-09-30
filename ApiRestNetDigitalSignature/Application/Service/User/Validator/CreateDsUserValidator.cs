using System.Collections;
using ApiRestNetDigitalSignature.Application.Service.User.Businessrules;
using ApiRestNetDigitalSignature.Dominio.Common;
using ApiRestNetDigitalSignature.Dominio.Model;
using ApiRestNetDigitalSignature.Dominio.Port;

namespace ApiRestNetDigitalSignature.Application.Service.User.Validator;

public class CreateDsUserValidator
{
    private IDsUserRepository _repository;

    public CreateDsUserValidator(IDsUserRepository repository)
    {
        _repository = repository;
    }

    public string validate(DsUser dsUser)
    {
        List<IValidator<DsUser>> validators = new List<IValidator<DsUser>>();
        validators.Add(new DsUserUserNameValidator(_repository));

        foreach (IValidator<DsUser> validator in validators)
        {
            string errorCode = validator.Validate(dsUser);

            if (!ErrorCode.SUCCESSFUL.Equals(errorCode))
            {
                return errorCode;
            }
        }

        return ErrorCode.SUCCESSFUL;
    }
}