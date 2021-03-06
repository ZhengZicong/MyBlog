﻿using System;
using System.Linq;
using Blog.Libraries.Core.Data;
using Blog.Libraries.Data.Domain.Member;

namespace Blog.Libraries.Services.Member
{
    public partial class AdminService : IAdminService
    {

        #region Fields

        private readonly IRepository<Admin> _adminRepository;

        #endregion
        
        public Admin GetAdminById(long adminId)
        {
            if (adminId == 0)
                return null;

            return _adminRepository.GetById(adminId);
        }

        public Admin GetAdminByGuid(Guid guid)
        {
            if (guid == Guid.Empty)
                return null;

            var query = from a in _adminRepository.Table
                        where a.Guid == guid
                        orderby a.Id
                        select a;

            return query.FirstOrDefault();
        }

    }

}
