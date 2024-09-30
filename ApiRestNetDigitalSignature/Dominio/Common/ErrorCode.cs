namespace ApiRestNetDigitalSignature.Dominio.Common;

public class ErrorCode
{

    public const string SUCCESSFUL = "API_DS-0000";
    public const string ERROR_PROCESSING_THE_TRANSACTION = "API_DS-0001";
    public const string CREATE_DS_USER_USER_NAME_IS_REQUIRED = "API_DS-0002";
    public const string CREATE_DS_USER_USER_NAME_IS_INVALID = "API_DS-0003";
    public const string CREATE_UPDATE_DS_USER_USER_NAME_ALREADY_EXIST = "API_DS-0004";
    public const string UPDATE_DS_USER_ID_NOT_FOUND = "API_DS-0005";
    public const string DELETE_DS_USER_ID_NOT_FOUND = "API_DS-0006";
    public const string SIGN_DOCUMENT_CERTIFICATE_FAIL_OPEN = "API_DS-0007";
    public const string DELETE_DS_DOCUMENT_ID_NOT_FOUND = "API_DS-0008";
    public const string DOWNLOAD_DS_DOCUMENT_PATH_INVALID = "API_DS-0009";
    public const string DOWNLOAD_DS_DOCUMENT_ID_INVALID = "API_DS-0010";

}