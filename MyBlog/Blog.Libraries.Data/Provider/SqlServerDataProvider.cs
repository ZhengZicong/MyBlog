﻿using System.Data.Common;
using System.Data.Entity.Infrastructure;
using Blog.Libraries.Core.Data;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Blog.Libraries.Data.Provider
{

    /// <summary>
    /// SqlServer数据提供者
    /// </summary>
    public class SqlServerDataProvider : IDataProvider
    {

        #region Properties

        /// <summary>
        /// SqlServer数据库支持存储过程
        /// </summary>
        public virtual bool StoredProceduredSupported
        {
            get { return true; }
        }

        /// <summary>
        /// SqlServer数据库支持备份
        /// </summary>
        public virtual bool BackupSupported
        {
            get { return true; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 初始化SqlServer数据库连接创建工厂
        /// </summary>
        public virtual void InitConnectionFactory()
        {
            var connectionFactory = new SqlConnectionFactory();
#pragma warning disable 618
            Database.DefaultConnectionFactory = connectionFactory;
#pragma warning restore 618
        }

        /// <summary>
        /// 初始化SqlServer数据库设置
        /// </summary>
        public virtual void SetDatabaseInitializer()
        {

        }

        /// <summary>
        /// 初始化SqlServer数据库
        /// </summary>
        public virtual void InitDatabase()
        {
            InitConnectionFactory();
            SetDatabaseInitializer();
        }

        /// <summary>
        /// 获取支持数据库参数对象 (用于存储过程)
        /// </summary>
        /// <returns>参数对象</returns>
        public virtual DbParameter GetParameter()
        {
            return new SqlParameter();
        }

        public virtual int SupportedLengthOfBinaryHash()
        {
            return 800;
        }

        #endregion

    }

}
