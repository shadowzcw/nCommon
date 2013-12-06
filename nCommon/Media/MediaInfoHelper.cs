using System;
using Common;

namespace nCommon.Media
{
    public class MediaInfoHelper
    {
        public static MediaInfomation GetMediaInfo(string path)
        {
            MediaInfo miDLL = new MediaInfo();
            try
            {
                miDLL.Open(path);
            }
            catch (System.Exception)
            {
                return null;
            }
            try
            {
                //返回一个概要性的信息 
                string info = miDLL.Inform();
                //获得媒体的播放时间，单位为ms（毫秒转化成秒、分、时需要自己手换算了）
                string duration = miDLL.Get(StreamKind.General, 0, "Duration");
                //获得文件长度(单位B)
                string filesize = miDLL.Get(0, 0, "FileSize");
                //获得媒体格式信息
                string format = miDLL.Get(StreamKind.General, 0, "Format");
                //码率
                string bitrate = miDLL.Get(0, 0, "BitRate");
                return SetMediaInformation(info, duration, filesize, format, bitrate);
            }
            catch (System.Exception)
            {
                return null;
            }
            finally
            {
                miDLL.Close();
            }
        }

        private static MediaInfomation SetMediaInformation(string info, string duration, string filesize, string format, string bitrate)
        {
            long tempDuration = string.IsNullOrEmpty(duration) ? 0 : Convert.ToInt64(duration) / 1000;
            long tempFileSize = string.IsNullOrEmpty(filesize) ? 0 : Convert.ToInt64(filesize) / 1024;
            int tempBitRate = string.IsNullOrEmpty(bitrate) ? 0 : Convert.ToInt32(bitrate) / 1024;

            MediaInfomation mi = new MediaInfomation(info, tempDuration, tempFileSize, format, tempBitRate);
            return mi;
        }
    }

    /// <summary>
    /// 媒体信息
    /// </summary>
    public class MediaInfomation
    {
        public MediaInfomation() { }

        public MediaInfomation(string info, long duration, long filesize, string format, int bitrate)
        {
            this.Info = info;
            this.Duration = duration;
            this.FileSize = filesize;
            this.Format = format;
            this.BitRate = bitrate;
        }
        /// <summary>
        ///  媒体信息
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 时长 秒
        /// </summary>
        public long Duration { get; set; }
        /// <summary>
        /// 文件大小 K
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// 媒体格式
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 码率 K
        /// </summary>
        public int BitRate { get; set; }
        /// <summary>
        /// 媒资资源类型
        /// </summary>
        public int MARType { get; set; }
        /// <summary>
        /// 媒资类型
        /// </summary>
        public int MAType { get; set; }
        /// <summary>
        /// 已经存在时长
        /// </summary>
        public int HasDuration { get; set; }
    }
}


