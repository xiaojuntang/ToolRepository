using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DesignPatterns
{
    public class FinalizeAndDispose
    {
    }

    public class ManagedRes : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class FileDealer : IDisposable
    {
        //定义一个访问文件资源的 Win32 句柄
        private IntPtr fileHandle;
        //定义引用的托管资源
        private ManagedRes managedRes;
        //定义构造器，初始化托管资源和非托管资源
        public FileDealer(IntPtr handle, ManagedRes res)
        {
            fileHandle = handle;
            managedRes = res;
        }
        //实现终结器，定义 Finalize
        ~FileDealer()
        {
            if (fileHandle != IntPtr.Zero)
            {
                Dispose(false);
            }
        }
        //实现 IDisposable 接口  释放托管资源
        public void Dispose()
        {
            Dispose(true);
            //阻止 GC 调用 Finalize 方法
            GC.SuppressFinalize(this);
        }
        //实现一个处理资源清理的具体方法
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //清理托管资源
                managedRes.Dispose();
            }
            //执行资源清理，在此为关闭对象句柄
            if (fileHandle != IntPtr.Zero)
            {
                CloseHandle(fileHandle);
                fileHandle = IntPtr.Zero;
            }
        }
        public void Close()
        {
            //在内部调用 Dispose 来实现
            Dispose();
        }
        //实现对文件句柄的其他应用方法
        public void Write() { }
        public void Read() { }
        //引入外部 Win32API
        [DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);
    }
}
