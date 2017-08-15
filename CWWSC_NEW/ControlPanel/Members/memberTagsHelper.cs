using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;

namespace Hidistro.ControlPanel.Commodities
{
    public class memberTagsHelper
    {

        /// <summary>
        /// 添加会员标签
        /// </summary>
        /// <param name="salesnfo">会员标签实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddmemberTags(memberTagsInfo tagInfo)
        {
            bool flag = new MemberTagsDao().AddmemberTags(tagInfo);
            return flag;
        }
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="TagID"></param>
        /// <returns></returns>
        public static memberTagsInfo GetMemberTagsInfo(int TagID)
        {
            return new MemberTagsDao().GetMemberTagsInfo(TagID);
        }
        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="displaysequence"></param>
        /// <returns></returns>
        public static bool updateSort(int TagID, int sort)
        {
            return new MemberTagsDao().updateSort(TagID, sort);
        }
        /// <summary>
        /// 根据条件查询data
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static DataTable GetmemberTagsData(string where = "")
        {
            return new MemberTagsDao().GetmemberTagsData(where);
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="TagID"></param>
        /// <returns></returns>
        public static bool DeleteMemberTags(int TagID)
        {
            return new MemberTagsDao().DeleteMemberTags(TagID);
        }
        /// <summary>
        /// 修改星会员标签
        /// </summary>
        /// <param name="salesnfo">会员标签实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool UpdateMemberTags(memberTagsInfo tagInfo)
        {
            bool flag = new MemberTagsDao().UpdateMemberTags(tagInfo);
            return flag;
        }

    }

}
