/************************************************************************************************************************
* 命名空间: Common.Net.Core.Attribute
* 项目描述:
* 版本名称: v1.0.0.0
* 作　　者: 唐晓军
* 所在区域: 北京
* 机器名称: DESKTOP-F6QRRBM
* 注册组织: 学科网（www.zxxk.com）
* 项目名称: 学易作业系统
* CLR版本:  4.0.30319.42000
* 创建时间: 2017/10/17 13:36:14
* 更新时间: 2017/10/17 13:36:14
* 
* 功 能： N/A
* 类 名： ArgumentValidationAttribute
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────────────────────────────
* V0.01 2017/10/17 13:36:14 唐晓军 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．                                                  │
*│　版权所有：北京凤凰学易科技有限公司　　　　　　　　　　　　　                                                      │
*└──────────────────────────────────────────────────────────┘
************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Core
{
    /// <summary>
    /// 参数验证
    /// </summary>
    public abstract class ArgumentValidationAttribute : Attribute
    {
        public abstract void Validate(object value, string argument);
    }

    /// <summary>
    /// 参数NULL验证 作者:唐晓军
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class NotNullAttribute : ArgumentValidationAttribute
    {
        public override void Validate(object value, string argument)
        {
            if (value == null)
                throw new ArgumentNullException(argument);
        }
    }

    /// <summary>
    /// 参数NotNullOrEmpty验证 作者:唐晓军
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class NotNullOrEmptyAttribute : ArgumentValidationAttribute
    {
        public override void Validate(object value, string argument)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
                throw new ArgumentNullException(argument);
        }
    }

    /// <summary>
    /// 范围验证 作者:唐晓军
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class InRangeAttribute : ArgumentValidationAttribute
    {
        private int min;
        private int max;

        public InRangeAttribute(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public override void Validate(object value, string argumentName)
        {
            int intValue = (int)value;
            if (intValue < min || intValue > max)
            {
                throw new ArgumentOutOfRangeException(argumentName, string.Format("min={0},max={1}", min, max));
            }
        }
    }
}
