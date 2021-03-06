﻿using System;
using Blog.Libraries.Data.Domain.Member;

namespace Blog.Libraries.Services.Member
{

    /// <summary>
    /// 管理员业务接口
    /// </summary>
    public partial interface IAdminService
    {

        /// <summary>
        /// 通过ID获取管理员
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <returns>一个管理员</returns>
        Admin GetAdminById(long adminId);

        /// <summary>
        /// 通过Guid获取管理员
        /// </summary>
        /// <param name="guid">Guid</param>
        /// <returns>管理员</returns>
        Admin GetAdminByGuid(Guid guid);

    }

}
