using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace nCommon.File
{
    public class FTPUploader : IUpload
    {
        private FtpWebRequest requestObj;
        private readonly string[] FtpConfig = System.Configuration.ConfigurationManager.AppSettings["n.Upload"].Split(new[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
        public bool Upload(HttpPostedFileBase file, string filename)
        {
            try
            {
                string uploadUrl = FtpConfig[0];

                Stream streamObj = file.InputStream;
                Byte[] buffer = new Byte[file.ContentLength];
                streamObj.Read(buffer, 0, buffer.Length);
                streamObj.Close();

                string ftpUrl = string.Format("{0}/{1}", uploadUrl, filename);
                requestObj = FtpWebRequest.Create(ftpUrl) as FtpWebRequest;
                requestObj.Method = WebRequestMethods.Ftp.UploadFile;
                requestObj.Credentials = new NetworkCredential(FtpConfig[1], FtpConfig[2]);
                Stream requestStream = requestObj.GetRequestStream();
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Flush();
                requestStream.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Upload(string filename, byte[] buffer)
        {
            try
            {
                string uploadUrl = FtpConfig[0];

                string ftpUrl = string.Format("{0}/{1}", uploadUrl, filename);
                requestObj = FtpWebRequest.Create(ftpUrl) as FtpWebRequest;
                requestObj.Method = WebRequestMethods.Ftp.UploadFile;
                requestObj.Credentials = new NetworkCredential(FtpConfig[1], FtpConfig[2]);
                Stream requestStream = requestObj.GetRequestStream();
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Flush();
                requestStream.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Exists(string filename)
        {
            try
            {
                string ftpUrl = string.Format("{0}/{1}", FtpConfig[0], filename);
                FtpWebRequest requestObj = FtpWebRequest.Create(ftpUrl) as FtpWebRequest;
                requestObj.Method = WebRequestMethods.Ftp.ListDirectory;
                requestObj.Credentials = new NetworkCredential(FtpConfig[1], FtpConfig[2]);
                FtpWebResponse response = (FtpWebResponse)requestObj.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();

                StringBuilder result = new StringBuilder();
                while (line != null)
                {
                    result.Append(line);

                    result.Append("\n");

                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);

                reader.Close();
                response.Close();
                return result.ToString().Split('\n').Length > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateDirectory(string foldername)
        {
            try
            {
                string ftpUrl = string.Format("{0}/{1}", FtpConfig[0], foldername);
                FtpWebRequest requestObj = FtpWebRequest.Create(ftpUrl) as FtpWebRequest;
                requestObj.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestObj.Credentials = new NetworkCredential(FtpConfig[1], FtpConfig[2]);
                FtpWebResponse response = (FtpWebResponse)requestObj.GetResponse();
                response.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string JoinSymbol { get { return "/"; } }
        public byte[] Download(string filename)
        {
            string ftpUrl = string.Format("{0}/{1}", FtpConfig[0], filename);
            FtpWebRequest requestObj = FtpWebRequest.Create(ftpUrl) as FtpWebRequest;
            requestObj.Method = WebRequestMethods.Ftp.DownloadFile;
            requestObj.Credentials = new NetworkCredential(FtpConfig[1], FtpConfig[2]);
            FtpWebResponse response = (FtpWebResponse)requestObj.GetResponse();
            Stream ftpStream = response.GetResponseStream();
            MemoryStream memoryStream = new MemoryStream();
            ftpStream.CopyTo(memoryStream);
            response.Close();
            ftpStream.Close();
            return memoryStream.ToArray();
        }
    }
}
