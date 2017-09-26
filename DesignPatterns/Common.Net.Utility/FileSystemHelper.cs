using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Utility
{
    public class FileSystemHelper
    {
        private string _dirPath = string.Empty;

        private string _filter = string.Empty;

        public FileSystemHelper(string dirPath, string filter)
        {
            this._dirPath = dirPath;
            this._filter = filter;
        }

        public void WatcherStrat(Action created, Action changed, Action deleted)
        {
            //Path——这个属性告诉FileSystemWatcher它需要监控哪条路径。例如，如果我们将这个属性设为“C: Temp”，对象就监控那个目录发生的所有改变。
            //IncludeSubDirectories——这个属性说明FileSystemWatcher对象是否应该监控子目录中发生的改变。
            //Filter——这个属性允许你过滤掉某些类型的文件发生的变化。例如，如果我们只希望在TXT文件被修改 / 新建 / 删除时提交通知，可以将这个属性设为“*txt”。在处理高流量或大型目录时，使用这个属性非常方便。

            //Changed——当被监控的目录中有一个文件被修改时，就提交这个事件。值得注意的是，这个事件可能会被提交多次，即使文件的内容仅仅发生一项改变。这是由于在保存文件时，文件的其它属性也发生了改变。
            //Created——当被监控的目录新建一个文件时，就提交这个事件。如果你计划用这个事件移动新建的事件，你必须在事件处理器中写入一些错误处理代码，它能处理当前文件被其它进程使用的情况。之所以要这样做，是因为Created事件可能在建立文件的进程释放文件之前就被提交。如果你没有准备正确处理这种情况的代码，就可能出现异常。
            //Deleted——当被监控的目录中有一个文件被删除，就提交这个事件。
            //Renamed——当被监控的目录中有一个文件被重命名，就提交这个事件。
            //注：如果你没有将EnableRaisingEvents设为真，系统不会提交任何一个事件。如果有时FileSystemWatcher对象似乎无法工作，请首先检查EnableRaisingEvents，确保它被设为真。

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = this._dirPath;
            watcher.Filter = this._filter;
            watcher.Changed += new FileSystemEventHandler(OnProcess);
            watcher.Created += new FileSystemEventHandler(OnProcess);
            watcher.Deleted += new FileSystemEventHandler(OnProcess);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.EnableRaisingEvents = true;
            //watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess
            //                       | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
            watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.Size;
            watcher.IncludeSubdirectories = true;
        }

        private void OnProcess(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                OnCreated(source, e);
            }
            else if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                OnChanged(source, e);
            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                OnDeleted(source, e);
            }
            else if (e.ChangeType == WatcherChangeTypes.Renamed)
            {
                OnRenamed(source, e as RenamedEventArgs);
            }
        }
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            Assembly assembly = Assembly.LoadFrom(e.FullPath);
            Console.WriteLine("文件新建事件处理逻辑{0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name);
        }
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Assembly assembly = Assembly.LoadFrom(e.FullPath);
            Console.WriteLine("文件改变事件处理逻辑{0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name);
        }
        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("文件删除事件处理逻辑{0}  {1}   {2}", e.ChangeType, e.FullPath, e.Name);
        }
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine("文件重命名事件处理逻辑{0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name);
        }
    }
}
