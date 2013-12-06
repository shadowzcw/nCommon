using System.Web;

namespace nCommon.File
{
    public interface IUpload
    {
        bool Upload(HttpPostedFileBase file, string filename);
        bool Upload(string filename, byte[] buffer);
        bool Exists(string filename);
        bool CreateDirectory(string foldername);
        string JoinSymbol { get; }
        byte[] Download(string filename);
    }
}
