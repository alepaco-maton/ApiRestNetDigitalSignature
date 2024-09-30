namespace ApiRestNetDigitalSignature.Dominio.Common;

using System;
using System.IO;

public class AppTools
{

    public static bool isBlank(string? value)
    {
        return value == null || (value.Trim().Length == 0);
    }

    public static void DeleteFile(string path, ILogger log)
    {
        FileInfo file = new FileInfo(path);

        if (file.Exists)
        {
            file.Delete();
        }
        else
        {
            log.LogWarning("El archivo no existe, {Path}", path);
        }
    }

    public static void DeleteDirectoryRecursively(string path)
    {
        DirectoryInfo directory = new DirectoryInfo(path);

        if (directory.Exists)
        {
            directory.Delete(true); // true para eliminar subdirectorios y archivos
        }
        else
        {
            Console.WriteLine($"El directorio {path} no existe.");
        }
    }

    internal static bool IsSuccessfulErrorCode(string errorCode)
    {
        return ErrorCode.SUCCESSFUL.Equals(errorCode);
    }
}