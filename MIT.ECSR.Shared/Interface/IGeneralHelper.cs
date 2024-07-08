using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using MIT.ECSR.Shared.Attributes;

namespace MIT.ECSR.Shared.Interface
{
    public interface IGeneralHelper
    {
        bool ValidatePassword(string password);
        string PasswordEncrypt(string text);
        (bool Success, string Message, string Original, string Resize) SaveImage(string target, Guid id, FileObject file, int width, int height);
        bool IsImage(string filename);
        bool IsFile(string base64);
        ObjectResponse<TokenObject> DecodeToken(string token);
        string DoubleToRupiah(double value);
    }
}

