

using ApiRestNetDigitalSignature.Dominio.Common;
using ApiRestNetDigitalSignature.Dominio.Model;
using ApiRestNetDigitalSignature.Dominio.Port;

namespace ApiRestNetDigitalSignature.Application.Service.User.Businessrules;

public class DsUserUserNameValidator : IValidator<DsUser>
{

    public const int USER_NAME_MIN_LENGHT = 3;
    public const int USER_NAME_MAX_LENGHT = 50;

    private IDsUserRepository repository;

    public DsUserUserNameValidator(IDsUserRepository repository)
    {
        this.repository = repository;
    }

    public string Validate(DsUser dsUser)
    {
        if (AppTools.isBlank(dsUser.UserName))
        {
            return ErrorCode.CREATE_DS_USER_USER_NAME_IS_REQUIRED;
        }

        int length = dsUser.UserName?.Trim().Length ?? 0;

        if (length < USER_NAME_MIN_LENGHT || length > USER_NAME_MAX_LENGHT)
        {
            return ErrorCode.CREATE_DS_USER_USER_NAME_IS_INVALID;
        }

        List<DsUser> list = repository.GetAllByUserName(dsUser.UserName);

        if (list.Any())
        {
            if (dsUser.Id == null)
            {
                return ErrorCode.CREATE_UPDATE_DS_USER_USER_NAME_ALREADY_EXIST;
            }
            else
            {
                if (!list.ElementAt(0).Id.Equals(dsUser.Id))
                {
                    return ErrorCode.CREATE_UPDATE_DS_USER_USER_NAME_ALREADY_EXIST;
                }
            }
        }

        return ErrorCode.SUCCESSFUL;
    }

}