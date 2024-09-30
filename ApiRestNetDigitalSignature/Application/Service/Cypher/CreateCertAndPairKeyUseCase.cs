
using ApiRestNetDigitalSignature.Dominio.Model;
using ApiRestNetDigitalSignature.Dominio.Port;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Logging;
using ApiRestNetDigitalSignature.Dominio.Common;

namespace ApiRestNetDigitalSignature.Application.Service.Cypher;

public class CreateCertAndPairKeyUseCase
{
    private const int PairKeySize = 2048;
    private const string PairKeyAlgorithm = "RSA";
    private readonly IAppLogger _logger;

    public CreateCertAndPairKeyUseCase(IAppLogger logger)
    {
        _logger = logger;
    }

    public DsUser Create(DsUser dsUser, string pathFolderByUser)
    {
        try
        {
            string pathFolder = Path.Combine(pathFolderByUser, dsUser.Id.ToString());

            dsUser.PrivateKey = Path.Combine(pathFolder, $"privateKey_{dsUser.Id}.key");
            dsUser.PublicKey = Path.Combine(pathFolder, $"publicKey_{dsUser.Id}.key");

            RSA rsa = CreateRSA(PairKeySize, pathFolderByUser, pathFolder, dsUser.PrivateKey, dsUser.PublicKey);

            dsUser.Cert = Path.Combine(pathFolder, $"cert_{dsUser.Id}.cer");

            CreateCertificate(dsUser.Cert, rsa);
        }
        catch (Exception e)
        {
            throw new DigitalSignatureException(ErrorCode.ERROR_PROCESSING_THE_TRANSACTION,
            e.Message, e);
        }

        return dsUser;
    }

    private string CreateCertificate(string certPath, RSA rsa)
    {
        try
        {
            // Usamos X509Certificate2 y RSA para crear un certificado autofirmado
            var certificateRequest = new CertificateRequest(
                "CN=Api Rest - Digital Signature",
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            certificateRequest.CertificateExtensions.Add(
                new X509BasicConstraintsExtension(false, false, 0, false));

            // Generar un certificado autofirmado
            var certificate = certificateRequest.CreateSelfSigned(
                DateTimeOffset.Now.AddMonths(-1),
                DateTimeOffset.Now.AddYears(1));

            // Guardar el certificado en un archivo .cer
            File.WriteAllBytes(certPath, certificate.Export(X509ContentType.Cert));

            return certPath;
        }
        catch (Exception ex)
        {
            _logger.Error("Error al crear el certificado.", ex);
            throw;
        }
    }

    private RSA CreateRSA(int keySize, string outputPath, string pathFolder,
    string privateKeyPath, string publicKeyPath)
    {
        try
        {
            var rsa = RSA.Create(keySize);

            CreateFolderIfNotExist(outputPath);
            CreateFolderIfNotExist(pathFolder);

            DeleteFileIfExists(privateKeyPath);
            DeleteFileIfExists(publicKeyPath);

            // Guardar la clave pública
            var publicKey = rsa.ExportRSAPublicKey();
            File.WriteAllBytes(publicKeyPath, publicKey);
            _logger.Info($"Clave pública guardada en: {publicKeyPath}");

            // Guardar la clave privada
            var privateKey = rsa.ExportRSAPrivateKey();
            File.WriteAllBytes(privateKeyPath, privateKey);
            _logger.Info($"Clave privada guardada en: {privateKeyPath}");

            return rsa;
        }
        catch (Exception ex)
        {
            _logger.Error("Error al generar el par de claves.", ex);
            throw;
        }
    }

    private void CreateFolderIfNotExist(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    private void DeleteFileIfExists(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            _logger.Info($"Archivo eliminado: {path}");
        }
    }
}
