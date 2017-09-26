/************************************************************************************************************************
* 命名空间: Common.Net.Core
* 项目描述:
* 版本名称: v1.0.0.0
* 作　　者: 唐晓军
* 所在区域: 北京
* 机器名称: DESKTOP-F6QRRBM
* 注册组织: 学科网（www.zxxk.com）
* 项目名称: 学易作业系统
* CLR版本:  4.0.30319.42000
* 创建时间: 2017/9/26 15:48:57
* 更新时间: 2017/9/26 15:48:57
* 
* 功 能： N/A
* 类 名： CheckArgument
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────────────────────────────
* V0.01 2017/9/26 15:48:57 唐晓军 初版
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
    /// 参数检查
    /// </summary>
    public class CheckArgument
    {
        /// <summary>
        /// 整型不能为零
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="input">值</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void IsNotZero(string paramName, int input)
        {
            if (input == 0)
            {
                throw new ArgumentException(string.Format("参数【{0}】不能等于零！", paramName));
            }
        }

        /// <summary>
        /// 整数大于零
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="input">值</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void IsGtZero(string paramName, int input)
        {
            if (input < 0)
            {
                throw new ArgumentException(string.Format("参数【{0}】不能小于零！", paramName));
            }
        }

        /// <summary>
        /// 整型的范围
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="input"></param>
        /// <param name="min">值</param>
        /// <param name="max"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void IntMaxAndMin(string paramName, int input, int min = 0, int max = 999999999)
        {
            if (input < min && input > max)
            {
                throw new ArgumentException(string.Format("参数【{0}】范围{1}-{2}间！", paramName, min, max));
            }
        }

        /// <summary>
        /// 对象不为NULL
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="obj">值</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void IsNotNull(string paramName, object obj)
        {
            if (obj == null)
            {
                throw new ArgumentException(string.Format("参数【{0}】不能等于NULL！", paramName));
            }
        }

        /// <summary>
        /// 检验字符串是否为空
        /// </summary>
        /// <param name="paramName">The name.</param>
        /// <param name="input">The input.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void IsNotNullOrEmpty(string paramName, string input)
        {
            if (string.IsNullOrEmpty(input.Trim()))
            {
                throw new ArgumentException(string.Format("参数【{0}】不能为空！", paramName));
            }
        }
    }
}
