using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Utility
{

    public interface IBaseInterface
    {
        string GetString();
    }

    internal class AssemblyCore
    {
        public string ActivedAssemblyName { get; private set; }

        public string CurrentType { get; private set; }

        public FileInfo DefaultAssemblyFile { get; private set; }

        public AssemblyCore(string assemblyName,string type)
        {
            ActivedAssemblyName = assemblyName;
            CurrentType = type;
            try
            {
                DefaultAssemblyFile = new FileInfo(assemblyName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    internal class AppDomainCore
    {
        public AppDomain DefaultAppDomain { get; private set; }
        public string DefaultAppDomainName { get; private set; }

        public AppDomainCore(string appDomainName)
        {
            DefaultAppDomainName = appDomainName;
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            setup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            Evidence evidence = new Evidence(AppDomain.CurrentDomain.Evidence);
            DefaultAppDomain = AppDomain.CreateDomain(appDomainName, evidence, setup);
        }

        public bool ClearAppDomain()
        {
            try
            {
                AppDomain.Unload(DefaultAppDomain);
                DefaultAppDomain = null;
                return true;
            }
            catch { return false; }
        }

        ~AppDomainCore()
        {
            ClearAppDomain();
        }
    }

    public class Proxy : IBaseInterface
    {
        AssemblyCore _assemblyCore;
        AppDomainCore _appDomainCore;

        public string DefaultAssemblyName { get { return _assemblyCore.ActivedAssemblyName; } }
        public string DefaultAppDomainName { get { return _appDomainCore.DefaultAppDomainName; } }

        public Proxy(string assemblyName, string typeName, string appDomainName)
        {
            _assemblyCore = new AssemblyCore(assemblyName, typeName);
            _appDomainCore = new AppDomainCore(appDomainName);
        }

        public void UnLoad()
        {
            _appDomainCore.ClearAppDomain();
        }

        IBaseInterface _proxy;
        public string GetString()
        {
            if (_proxy == null)
                _proxy = _appDomainCore.DefaultAppDomain.CreateInstanceFromAndUnwrap(
                    _assemblyCore.ActivedAssemblyName, _assemblyCore.CurrentType)
                    as IBaseInterface;
            return _proxy.GetString();
        }
    }
}
