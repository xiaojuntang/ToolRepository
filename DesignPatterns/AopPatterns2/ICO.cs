/************************************************************************************************************************
* 命名空间: AopPatterns2
* 项目描述: 
* 版本名称: v1.0.0.0
* 作　　者: 唐晓军（QQ:417281862）
* 所在区域: 北京
* 机器名称: DESKTOP-F6QRRBM
* 注册组织: 学科网（www.zxxk.com）
* 项目名称: 学易作业系统
* CLR版本:  4.0.30319.42000
* 创建时间: 2017/8/22 11:37:53
* 更新时间: 2017/8/22 11:37:53
* 
* 功 能： N/A
* 类 名： ICO
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────────────────────────────
* V0.01 2017/8/22 11:37:53 唐晓军 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．                                                  │
*│　版权所有：北京凤凰学易科技有限公司　　　　　　　　　　　　　                                                      │
*└──────────────────────────────────────────────────────────┘
************************************************************************************************************************/
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AopPatterns2
{
    class ICO
    {

        public void Init()
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(Dal<>)).As(typeof(Idal<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            //builder.Register(c => new PersionBll((IRepository<Persion>)c.Resolve(typeof(IRepository<Persion>))));
            //上面的那些类如果在单独的工程里,如生成的程序集为AutofacUnitTest，就可以使用
            //Assembly.Load("AutofacUnitTest")获得响应的程序集。如果所有的文件在一个控制台程序里，
            //可以通过Assembly.GetExecutingAssembly();　直接获得相应的程序集。
            //Assembly dataAccess = Assembly.Load("AutofacUnitTest");
            //builder.RegisterAssemblyTypes(dataAccess)
            //        .Where(t => typeof(IDependency).IsAssignableFrom(t) && t.Name.EndsWith("Bll"));
            //RegisterAssemblyTypes方法将实现IDependency接口并已Bll结尾的类都注册了，语法非常的简单

            //builder.RegisterGeneric(typeof(Dal<>)).As(typeof(Idal<>)).InstancePerDependency(); 通过AS可以让类中通过构造函数依赖注入类型相应的接口。(当然也可以使用builder.RegisterType<类>().As<接口>(); 来注册不是泛型的类
            builder.Register(c => new CustomBll((IRepository<Custom>)c.Resolve(typeof(IRepository<Custom>))));

            var container = builder.Build();
        }

    }

    public interface IDependency { }


    public class CustomBll : IDependency
    {
        private readonly IRepository<Custom> _customRepostiory;
        public CustomBll(IRepository<Custom> _customRepostiory)
        {
            this._customRepostiory = _customRepostiory;
        }

        public void Insert(Custom c)
        {
            this._customRepostiory.Insert(c);
        }
    }


    public interface Idal<T> where T : class
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

    public class Dal<T> : Idal<T> where T : class
    {
        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }


    public interface IRepository<T> where T : class
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }



























    public class Persion
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class Custom
    {
        public string CustomName { get; set; }
        public int CustomID { get; set; }
    }
}
