using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Core
{
    /// <summary>
    /// 分页条
    /// </summary>
    public class PageBar
    {
        /// <summary>
        /// 分页栏HTML代码
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="totalCount">总数量</param>
        /// <returns></returns>
        public static string ShowPageBar(int pageSize, int? pageIndex, int totalCount)
        {
            pageIndex = pageIndex ?? 1;
            pageSize = (pageSize == 0 ? 8 : pageSize);
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder();
            if (totalPages > 1)
            {
                if (pageIndex == 1) //第一页
                {
                    // output.Append(" <a disabled='disabled'>&lt;&lt;</a> ");//首页
                    // output.Append(" <a disabled='disabled'>&lt;</a> ");//上一页
                    output.Append(" <a disabled='disabled' class='colH'>上一页</a> ");//上一页
                }
                if (pageIndex > 1)
                {
                    //处理首页连接
                    //处理上一页的连接
                    // output.Append(" <a data-pageIndex='1' class='pageLink'>&lt;&lt;</a> ");//首页
                    // output.AppendFormat(" <a data-pageIndex='{0}' class='pageLink'>&lt;</a> ", currentPage - 1);//上一页
                    output.AppendFormat(" <a data-pageIndex='{0}' class='pageLink'>上一页</a> ", pageIndex - 1);//上一页
                }
                if (totalPages > 7)
                {
                    int currint = 3;
                    if (pageIndex < 4)
                    {
                        for (int i = 0; i <= 6; i++)
                        {
                            if (pageIndex == i + 1)
                            {
                                // output.AppendFormat(" <a class='show'>{0}</a> ", currentPage);
                                output.AppendFormat(" <a class='cur'>{0}</a> ", pageIndex);
                            }
                            else
                            {
                                if (i == 6)
                                {
                                    output.AppendFormat(" <a data-pageIndex='{0}' class='pageLink'>...</a> ", 7);
                                    output.AppendFormat(" <a data-pageIndex='{0}' class='pageLink'>{0}</a> ", totalPages);//最后页
                                }
                                else
                                {
                                    output.AppendFormat(" <a data-pageIndex='{0}' class='pageLink'>{0}</a> ", i + 1);
                                }
                            }
                        }
                    }
                    else if (pageIndex >= 4 && pageIndex < totalPages - 3)
                    {
                        for (int i = 0; i <= 6; i++)
                        {
                            if (i == 0)
                            {
                                output.AppendFormat(" <a data-pageIndex='{0}' class='pageLink'>{0}</a> ", 1);//首页
                                output.AppendFormat(" <a data-pageIndex='{0}' class='pageLink'>...</a> ", pageIndex - 3);

                            }
                            else if (i == 3)//中间当前页
                            {
                                // output.AppendFormat(" <a class='show'>{0}</a> ", currentPage);
                                output.AppendFormat(" <a class='cur'>{0}</a> ", pageIndex);
                            }
                            else if (i == 6)
                            {
                                output.AppendFormat(" <a data-pageIndex='{0}' class='pageLink'>...</a> ", pageIndex + 3);
                                output.AppendFormat(" <a data-pageIndex='{0}' class='pageLink'>{0}</a> ", totalPages);//最后页
                            }
                            else
                            {
                                output.AppendFormat("  <a data-pageIndex='{0}' class='pageLink'>{0}</a> ", pageIndex + i - currint);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= 6; i++)
                        {
                            if (i == 0)
                            {
                                output.AppendFormat(" <a data-pageIndex='{0}' class='pageLink'>{0}</a> ", 1);//首页
                                output.AppendFormat(" <a data-pageIndex='{0}' class='pageLink'>...</a> ", totalPages - 6);
                            }
                            else
                            {
                                if (totalPages - 6 + i == pageIndex)
                                {
                                    // output.AppendFormat(" <a class='show'>{0}</a> ", currentPage);
                                    output.AppendFormat(" <a class='cur'>{0}</a> ", pageIndex);
                                }
                                else
                                {
                                    output.AppendFormat(" <a data-pageIndex='{0}' class='pageLink'>{0}</a> ", totalPages - 6 + i);
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < totalPages; i++)
                    {
                        if (pageIndex == i + 1)
                        {
                            // output.AppendFormat(" <a class='show'>{0}</a> ", currentPage);
                            output.AppendFormat(" <a class='cur'>{0}</a> ", pageIndex);
                        }
                        else
                        {
                            output.AppendFormat("  <a data-pageIndex='{0}' class='pageLink'>{0}</a> ", i + 1);
                        }
                    }
                }
                if (pageIndex == totalPages) //最后一页
                {
                    //处理下一页和尾页的链接
                    // output.Append(" <a disabled='disabled'>&gt;</a> ");
                    // output.Append(" <a disabled='disabled'>&gt;&gt;</a> ");
                    output.Append(" <a disabled='disabled' class='colH'>下一页</a> ");
                }
                if (pageIndex < totalPages)
                {
                    //处理下一页和尾页的链接 
                    // output.AppendFormat(" <a data-pageindex='{0}' class='pageLink'>&gt;</a> ", currentPage + 1);
                    // output.AppendFormat(" <a data-pageindex='{0}' class='pageLink'>&gt;&gt;</a> ", totalPages);
                    output.AppendFormat(" <a data-pageindex='{0}' class='pageLink'>下一页</a> ", pageIndex + 1);
                }
            }
            return output.ToString();
        }
    }
}
