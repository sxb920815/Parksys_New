﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCLYGV3.DB
{
	public class DBContext : DbContext
	{


		public DbSet<M_AdminUser> AdminUserList { get; set; }
		public DbSet<M_Permission> PermissionList { get; set; }
		public DbSet<M_PermissionOperation> PermissionOperationList { get; set; }
		public DbSet<M_Role> RoleList { get; set; }
		public DbSet<M_ItemInfo> ItemInfoList { get; set; }
		public DbSet<M_SysFile> SysFileList { get; set; }


		public DbSet<M_Area> Areaist { get; set; }
		public DbSet<M_AreaAndEqu> AreaAndEquList { get; set; }
		public DbSet<M_BreakRuleAnnal> BreakRuleAnnalList { get; set; }
		public DbSet<M_Car> CarList { get; set; }
		public DbSet<M_CardAnnal> CardAnnalList { get; set; }
		public DbSet<M_Equipment> EquipmentList { get; set; }
		public DbSet<M_Wave> WaveList { get; set; }
		public DbSet<M_WaveAnnal> WaveAnnalList { get; set; }
        public DBContext()
		{
			Database.SetInitializer<DBContext>(new CreateDatabaseIfNotExists<DBContext>());

            /* 策略一：数据库不存在时重新创建数据库
			 * Database.SetInitializer<testContext>(new CreateDatabaseIfNotExists<DBContext>());
			 * 
			 * 策略二：每次启动应用程序时创建数据库
			 * Database.SetInitializer<testContext>(new DropCreateDatabaseAlways<DBContext>());
			 * 
			 * 策略三：模型更改时重新创建数据库
			 * Database.SetInitializer<testContext>(new DropCreateDatabaseIfModelChanges<DBContext>());
			 * 
			 * 策略四：从不创建数据库
			 * Database.SetInitializer<testContext>(null);
			 * 
			 * 
			 * 
			*/
            //
        }

        public DBContext(string conn)
			: base(conn)
		{
			//是否启用延迟加载:  
			//  true:   延迟加载（Lazy Loading）：获取实体时不会加载其导航属性，一旦用到导航属性就会自动加载  
			//  false:  直接加载（Eager loading）：通过 Include 之类的方法显示加载导航属性，获取实体时会即时加载通过 Include 指定的导航属性  
			this.Configuration.LazyLoadingEnabled = true;

			this.Configuration.AutoDetectChangesEnabled = true;  //自动监测变化，默认值为 true  
		}

		/// <summary>  
		/// 实体到数据库结构的映射是通过默认的约定来进行的，如果需要修改的话，有两种方式，分别是：Data Annotations 和 Fluent API  
		//  以下示范通过 Fluent API 来修改实体到数据库结构的映射  
		/// </summary>  
		/// <param name="modelBuilder"></param>  
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
