using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

/// <summary>
/// Descripción breve de Encryptacion64
/// </summary>
public class Encryptacion64
{
    public string b64encode(string StrEncode)
    {
        string encodedString;
        try
        {
            encodedString = (Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(StrEncode)));
        }
        catch (Exception ex)
        {
            encodedString = string.Empty;
        }
        return encodedString;
    }

    public string b64decode(string StrDecode)
    {
        string decodedString;
        try
        {
            decodedString = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(StrDecode));
        }
        catch (Exception ex)
        {
            decodedString = string.Empty;
        }
        return decodedString;
    }
}
