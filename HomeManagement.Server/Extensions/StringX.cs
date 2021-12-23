using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace HomeManagement.Server.Extensions;

public static class StringX
{
    public static string Hash( this string password )
    {
        using var sha = SHA256.Create();
        byte[] hashed = sha.ComputeHash( Encoding.UTF8.GetBytes( password ) );
        return BitConverter.ToString( hashed ).Replace( "-", "" ).ToLower();
    }
}