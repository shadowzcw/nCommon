using System;
using System.Runtime.InteropServices;

// MediaInfoDLL - All info about media files, for DLL 
// Copyright (C) 2002-2009 Jerome Martinez, Zen@MediaArea.net 
//  
// This library is free software; you can redistribute it and/or 
// modify it under the terms of the GNU Lesser General Public 
// License as published by the Free Software Foundation; either 
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful, 
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU 
// Lesser General Public License for more details. 
//
// MediaInfoDLL - All info about media files, for DLL 
// Copyright (C) 2002-2009 Jerome Martinez, Zen@MediaArea.net 
// 
// This library is free software; you can redistribute it and/or 
// modify it under the terms of the GNU Lesser General Public 
// License as published by the Free Software Foundation; either  
// version 2.1 of the License, or (at your option) any later version.  
//  
// This library is distributed in the hope that it will be useful,  
// but WITHOUT ANY WARRANTY; without even the implied warranty of  
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  
// Lesser General Public License for more details.  
//  
// You should have received a copy of the GNU Lesser General Public  
// License along with this library; if not, write to the Free Software  
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA  
// 
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++  
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++  
//  
// Microsoft Visual C# wrapper for MediaInfo Library  
// See MediaInfo.h for help  
//  
// To make it working, you must put MediaInfo  
// in the executable folder  
//  
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++    
namespace Common
{
    public enum StreamKind
    {
        General,
        Video,
        Audio,
        Text,
        Chapters,
        Image
    }
    public enum InfoKind
    {
        Name,
        Text,
        Measure,
        Options,
        NameText,
        MeasureText,
        Info,
        HowTo
    }
    public enum InfoOptions
    {
        ShowInInform,
        Support,
        ShowInSupported,
        TypeOfValue
    }
    public enum InfoFileOptions
    {
        FileOption_Nothing = 0x00,
        FileOption_NoRecursive = 0x01,
        FileOption_CloseAll = 0x02,
        FileOption_Max = 0x04
    };
    public class MediaInfo
    {
        //Import of DLL functions. DO NOT USE until you know what you do (MediaInfo DLL do NOT use CoTaskMemAlloc to allocate memory)
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfo_New();
        [DllImport("MediaInfo.dl")]
        private static extern void MediaInfo_Delete(IntPtr Handle);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfo_Open(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string FileName);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoA_Open(IntPtr Handle, IntPtr FileName);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfo_Open_Buffer_Init(IntPtr Handle, Int64 File_Size, Int64 File_Offset);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoA_Open(IntPtr Handle, Int64 File_Size, Int64 File_Offset);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfo_Open_Buffer_Continue(IntPtr Handle, IntPtr Buffer, IntPtr Buffer_Size);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoA_Open_Buffer_Continue(IntPtr Handle, Int64 File_Size, byte[] Buffer, IntPtr Buffer_Size);
        [DllImport("MediaInfo.dl")]
        private static extern Int64 MediaInfo_Open_Buffer_Continue_GoTo_Get(IntPtr Handle);
        [DllImport("MediaInfo.dl")]
        private static extern Int64 MediaInfoA_Open_Buffer_Continue_GoTo_Get(IntPtr Handle);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfo_Open_Buffer_Finalize(IntPtr Handle);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoA_Open_Buffer_Finalize(IntPtr Handle);
        [DllImport("MediaInfo.dl")]
        private static extern void MediaInfo_Close(IntPtr Handle);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfo_Inform(IntPtr Handle, IntPtr Reserved);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoA_Inform(IntPtr Handle, IntPtr Reserved);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfo_GetI(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, IntPtr Parameter, IntPtr KindOfInfo);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoA_GetI(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, IntPtr Parameter, IntPtr KindOfInfo);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfo_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, [MarshalAs(UnmanagedType.LPWStr)] string Parameter, IntPtr KindOfInfo, IntPtr KindOfSearch);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoA_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber, IntPtr Parameter, IntPtr KindOfInfo, IntPtr KindOfSearch);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfo_Option(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string Option, [MarshalAs(UnmanagedType.LPWStr)] string Value);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoA_Option(IntPtr Handle, IntPtr Option, IntPtr Value);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfo_State_Get(IntPtr Handle);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfo_Count_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber);
        //MediaInfo class          
        public MediaInfo()
        {
            Handle = MediaInfo_New();
            if (Environment.OSVersion.ToString().IndexOf("Windows") == -1) MustUseAnsi = true; else MustUseAnsi = false;
        }
        ~MediaInfo()
        {
            MediaInfo_Delete(Handle);
        }
        public int Open(String FileName)
        {
            if (MustUseAnsi)
            {
                IntPtr FileName_Ptr = Marshal.StringToHGlobalAnsi(FileName);
                int ToReturn = (int)MediaInfoA_Open(Handle, FileName_Ptr);
                Marshal.FreeHGlobal(FileName_Ptr);
                return ToReturn;
            }
            else return (int)MediaInfo_Open(Handle, FileName);
        }
        public int Open_Buffer_Init(Int64 File_Size, Int64 File_Offset)
        {
            return (int)MediaInfo_Open_Buffer_Init(Handle, File_Size, File_Offset);
        }
        public int Open_Buffer_Continue(IntPtr Buffer, IntPtr Buffer_Size)
        {
            return (int)MediaInfo_Open_Buffer_Continue(Handle, Buffer, Buffer_Size);
        }
        public Int64 Open_Buffer_Continue_GoTo_Get()
        {
            return (int)MediaInfo_Open_Buffer_Continue_GoTo_Get(Handle);
        }
        public int Open_Buffer_Finalize()
        {
            return (int)MediaInfo_Open_Buffer_Finalize(Handle);
        }
        public void Close()
        {
            MediaInfo_Close(Handle);
        }
        public String Inform()
        {
            if (MustUseAnsi) return Marshal.PtrToStringAnsi(MediaInfoA_Inform(Handle, (IntPtr)0));
            else return Marshal.PtrToStringUni(MediaInfo_Inform(Handle, (IntPtr)0));
        }
        public String Get(StreamKind StreamKind, int StreamNumber, String Parameter, InfoKind KindOfInfo, InfoKind KindOfSearch)
        {
            if (MustUseAnsi)
            {
                IntPtr Parameter_Ptr = Marshal.StringToHGlobalAnsi(Parameter);
                String ToReturn = Marshal.PtrToStringAnsi(MediaInfoA_Get(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber, Parameter_Ptr, (IntPtr)KindOfInfo, (IntPtr)KindOfSearch));
                Marshal.FreeHGlobal(Parameter_Ptr); return ToReturn;
            }
            else return Marshal.PtrToStringUni(MediaInfo_Get(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber, Parameter, (IntPtr)KindOfInfo, (IntPtr)KindOfSearch));
        }
        public String Get(StreamKind StreamKind, int StreamNumber, int Parameter, InfoKind KindOfInfo)
        {
            if (MustUseAnsi) return Marshal.PtrToStringAnsi(MediaInfoA_GetI(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber, (IntPtr)Parameter, (IntPtr)KindOfInfo));
            else return Marshal.PtrToStringUni(MediaInfo_GetI(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber, (IntPtr)Parameter, (IntPtr)KindOfInfo));
        }
        public String Option(String Option, String Value)
        {
            if (MustUseAnsi)
            {
                IntPtr Option_Ptr = Marshal.StringToHGlobalAnsi(Option);
                IntPtr Value_Ptr = Marshal.StringToHGlobalAnsi(Value);
                String ToReturn = Marshal.PtrToStringAnsi(MediaInfoA_Option(Handle, Option_Ptr, Value_Ptr));
                Marshal.FreeHGlobal(Option_Ptr);
                Marshal.FreeHGlobal(Value_Ptr);
                return ToReturn;
            }
            else return Marshal.PtrToStringUni(MediaInfo_Option(Handle, Option, Value));
        }
        public int State_Get()
        {
            return (int)MediaInfo_State_Get(Handle);
        }
        public int Count_Get(StreamKind StreamKind, int StreamNumber)
        {
            return (int)MediaInfo_Count_Get(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber);
        }
        private IntPtr Handle;
        private bool MustUseAnsi;
        //Default values, if you know how to set default values in C#, say me          
        public String Get(StreamKind StreamKind, int StreamNumber, String Parameter, InfoKind KindOfInfo)
        {
            return Get(StreamKind, StreamNumber, Parameter, KindOfInfo, InfoKind.Name);
        }
        public String Get(StreamKind StreamKind, int StreamNumber, String Parameter)
        {
            return Get(StreamKind, StreamNumber, Parameter, InfoKind.Text, InfoKind.Name);
        }
        public String Get(StreamKind StreamKind, int StreamNumber, int Parameter)
        {
            return Get(StreamKind, StreamNumber, Parameter, InfoKind.Text);
        }
        public String Option(String Option_)
        {
            return Option(Option_, "");
        }
        public int Count_Get(StreamKind StreamKind)
        {
            return Count_Get(StreamKind, -1);
        }

    }
    public class MediaInfoList
    {
        //Import of DLL functions. DO NOT USE until you know what you do (MediaInfo DLL do NOT use CoTaskMemAlloc to allocate memory
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoList_New();
        [DllImport("MediaInfo.dl")]
        private static extern void MediaInfoList_Delete(IntPtr Handle);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoList_Open(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string FileName, IntPtr Options);
        [DllImport("MediaInfo.dl")]
        private static extern void MediaInfoList_Close(IntPtr Handle, IntPtr FilePos);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoList_Inform(IntPtr Handle, IntPtr FilePos, IntPtr Reserved);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoList_GetI(IntPtr Handle, IntPtr FilePos, IntPtr StreamKind, IntPtr StreamNumber, IntPtr Parameter, IntPtr KindOfInfo);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoList_Get(IntPtr Handle, IntPtr FilePos, IntPtr StreamKind, IntPtr StreamNumber, [MarshalAs(UnmanagedType.LPWStr)] string Parameter, IntPtr KindOfInfo, IntPtr KindOfSearch);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoList_Option(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string Option, [MarshalAs(UnmanagedType.LPWStr)] string Value);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoList_State_Get(IntPtr Handle);
        [DllImport("MediaInfo.dl")]
        private static extern IntPtr MediaInfoList_Count_Get(IntPtr Handle, IntPtr FilePos, IntPtr StreamKind, IntPtr StreamNumber);
        //MediaInfo class          
        public MediaInfoList()
        {
            Handle = MediaInfoList_New();
        }
        ~MediaInfoList()
        {
            MediaInfoList_Delete(Handle);
        }
        public int Open(String FileName, InfoFileOptions Options)
        {
            return (int)MediaInfoList_Open(Handle, FileName, (IntPtr)Options);
        }
        public void Close(int FilePos)
        {
            MediaInfoList_Close(Handle, (IntPtr)FilePos);
        }
        public String Inform(int FilePos)
        {
            return Marshal.PtrToStringUni(MediaInfoList_Inform(Handle, (IntPtr)FilePos, (IntPtr)0));
        }
        public String Get(int FilePos, StreamKind StreamKind, int StreamNumber, String Parameter, InfoKind KindOfInfo, InfoKind KindOfSearch)
        {
            return Marshal.PtrToStringUni(MediaInfoList_Get(Handle, (IntPtr)FilePos, (IntPtr)StreamKind, (IntPtr)StreamNumber, Parameter, (IntPtr)KindOfInfo, (IntPtr)KindOfSearch));
        }

        public String Get(int FilePos, StreamKind StreamKind, int StreamNumber, int Parameter, InfoKind KindOfInfo)
        {
            return Marshal.PtrToStringUni(MediaInfoList_GetI(Handle, (IntPtr)FilePos, (IntPtr)StreamKind, (IntPtr)StreamNumber, (IntPtr)Parameter, (IntPtr)KindOfInfo));
        }
        public String Option(String Option, String Value)
        {
            return Marshal.PtrToStringUni(MediaInfoList_Option(Handle, Option, Value));
        }
        public int State_Get()
        {
            return (int)MediaInfoList_State_Get(Handle);
        }
        public int Count_Get(int FilePos, StreamKind StreamKind, int StreamNumber)
        {
            return (int)MediaInfoList_Count_Get(Handle, (IntPtr)FilePos, (IntPtr)StreamKind, (IntPtr)StreamNumber);
        }
        private IntPtr Handle;
        //Default values, if you know how to set default values in C#, say me
        public void Open(String FileName)
        {
            Open(FileName, 0);
        }
        public void Close()
        {
            Close(-1);
        }
        public String Get(int FilePos, StreamKind StreamKind, int StreamNumber, String Parameter, InfoKind KindOfInfo)
        {
            return Get(FilePos, StreamKind, StreamNumber, Parameter, KindOfInfo, InfoKind.Name);
        }
        public String Get(int FilePos, StreamKind StreamKind, int StreamNumber, String Parameter)
        {
            return Get(FilePos, StreamKind, StreamNumber, Parameter, InfoKind.Text, InfoKind.Name);
        }
        public String Get(int FilePos, StreamKind StreamKind, int StreamNumber, int Parameter)
        {
            return Get(FilePos, StreamKind, StreamNumber, Parameter, InfoKind.Text);
        }
        public String Option(String Option_)
        {
            return Option(Option_, "");
        }
        public int Count_Get(int FilePos, StreamKind StreamKind)
        {
            return Count_Get(FilePos, StreamKind, -1);
        }
    }

    //public class MediaInfoSystem1
    //{
    //    public void ReadMediaInfo()
    //    {
    //        try
    //        {
    //            MediaInfo media = new MediaInfo();
    //            media.Open(@"E:\Movie\【更多电影请去www.dy131.com】超时空要塞F虚空歌姬BD中字1024x576版.rmvb");
    //            //取一个文件的信息必须先open，用完后再Close。切记。
    //            try
    //            {
    //                //返回一个概要性的信息
    //                string info = media.Inform();
    //                //获得媒体的播放时间，单位为ms（毫秒转化成秒、分、时需要自己手换算了）
    //                string duration = media.Get(StreamKind.General, 0, "Duration");
    //                //获得文件长度(单位B)
    //                string filesize = media.Get(0, 0, "FileSize");
    //                //获得媒体格式信息
    //                string fmt = media.Get(StreamKind.General, 0, "Format");
    //                //bitrate
    //                string bitrate = media.Get(0, 0, "BitRate");
    //            }
    //            finally
    //            {
    //                media.Close();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //    }
    //}
}
