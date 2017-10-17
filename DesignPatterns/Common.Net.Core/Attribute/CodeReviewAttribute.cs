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
* 创建时间: 2017/10/17 9:00:37
* 更新时间: 2017/10/17 9:00:37
* 
* 功 能： N/A
* 类 名： CodeReviewAttribute
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────────────────────────────
* V0.01 2017/10/17 9:00:37 唐晓军 初版
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
    /// 代码检测
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CodeReviewAttribute : Attribute
    {
        private string reviewer;

        private string date;

        private string comment;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="reviewer"></param>
        /// <param name="date"></param>
        /// <param name="comment"></param>
        public CodeReviewAttribute(string reviewer, string date, string comment = "")
        {
            this.reviewer = reviewer;
            this.date = date;
            this.comment = comment;
        }
        /// <summary>
        /// 检查人
        /// </summary>
        public string Reviewer { get => reviewer; set => reviewer = value; }
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get => date; set => date = value; }
        /// <summary>
        /// 检查结果
        /// </summary>
        public string Comment { get => comment; set => comment = value; }
    }
}
