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
    public class memberTagsRelationHelper
    {

        /// <summary>
        /// 添加会员标签关系
        /// </summary>
        /// <param name="salesnfo">会员标签实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool AddmemberTags(memberTagsRelationInfo tagInfo)
        {
            bool flag = new memberTagsRelationDao().AddmemberTags(tagInfo);
            return flag;
        }
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="TagID"></param>
        /// <returns></returns>
        public static memberTagsRelationInfo GetMemberTagsInfo(int MtID)
        {
            return new memberTagsRelationDao().GetMemberTagsInfo(MtID);
        }
        /// <summary>
        /// 根据条件查询data
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static DataTable GetmemberTagsData(string where = "")
        {
            return new memberTagsRelationDao().GetmemberTagsData(where);
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="TagID"></param>
        /// <returns></returns>
        public static bool DeleteMemberTags(int MtID)
        {
            return new memberTagsRelationDao().DeleteMemberTags(MtID);
        }
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="TagID"></param>
        /// <returns></returns>
        public static bool DeleteMemberTagsWhere(string where="")
        {
            return new memberTagsRelationDao().DeleteMemberTagsWhere(where);
        }
        /// <summary>
        /// 获取当前用户的标签数
        /// </summary>
        public static int GetmemberTagsNum(string userId)
        {
            return new memberTagsRelationDao().GetmemberTagsNum(userId);
           
        }
        /// <summary>
        /// 修改星会员标签关系
        /// </summary>
        /// <param name="salesnfo">会员标签实体对象</param>
        /// <returns>true添加成功，false添加失败</returns>
        public static bool UpdateMemberTags(memberTagsRelationInfo tagInfo)
        {
            bool flag = new memberTagsRelationDao().UpdateMemberTags(tagInfo);
            return flag;
        }

    }

}
