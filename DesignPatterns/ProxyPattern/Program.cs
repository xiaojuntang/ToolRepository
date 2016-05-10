using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyPattern
{
    /// <summary>
    /// http://www.cnblogs.com/hegezhou_hot/archive/2011/02/20/1958965.html
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            PlayProxy proxy = new PlayProxy();
            proxy.Play(FileEnum.MP3);
        }
    }

    /// <summary>
    /// 定义客户端调用的具体的音乐媒体类型类型
    /// </summary>
    public enum FileEnum
    {
        MP3,
        APE,
        WMA,
        MP4
    }

    /// <summary>
    /// 定义客户端调用的目标对象的接口
    /// </summary>
    public interface IPlay
    {
        bool Play(FileEnum fileType);
    }

    /// <summary>
    /// 定义具体目标类型的实现
    /// </summary>
    public class PlaySubject : IPlay
    {
        private static readonly PlaySubject instance = new PlaySubject();

        public PlaySubject() { }

        public static IPlay PlaySub
        {
            get { return instance; }
        }

        public bool Play(FileEnum fileType)
        {
            if (fileType == FileEnum.MP3)
                return true;
            return false;
        }
    }

    public class PlayProxy : IPlay
    {
        public bool Play(FileEnum fileType)
        {
            return PlaySubject.PlaySub.Play(fileType);
        }
    }
}
