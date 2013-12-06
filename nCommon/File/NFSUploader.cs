using System.Configuration;
using System.Web;

namespace nCommon.File
{
    public class NFSUploader : IUpload
    {
        private string NUpload { get { return ConfigurationManager.AppSettings["n.Upload"]; } }
        public bool Upload(HttpPostedFileBase file, string filename)
        {
            try
            {
                var fullPath = NUpload + "\\" + filename;
                file.SaveAs(fullPath);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public bool Upload(string filename, byte[] buffer)
        {
            try
            {
                var fullPath = NUpload + "\\" + filename;
                System.IO.File.WriteAllBytes(fullPath, buffer);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool Exists(string filename)
        {
            var fullPath = NUpload + "\\" + filename;
            return System.IO.Directory.Exists(fullPath);
        }

        public bool CreateDirectory(string foldername)
        {
            try
            {
                var fullPath = NUpload + "\\" + foldername;
                if (!System.IO.Directory.Exists(fullPath)) System.IO.Directory.CreateDirectory(fullPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string JoinSymbol { get { return "\\"; } }

        public byte[] Download(string filename)
        {
            var fullPath = NUpload + "\\" + filename;
            return System.IO.File.ReadAllBytes(fullPath);
        }
    }
}
