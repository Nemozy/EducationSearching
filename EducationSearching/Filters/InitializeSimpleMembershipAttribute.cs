﻿using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using EducationSearching.Models;

namespace EducationSearching.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Обеспечение однократной инициализации ASP.NET Simple Membership при каждом запуске приложения
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                WebSecurity.InitializeDatabaseConnection("DB_accountsConnectionString", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                /*//Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new DBDataContext())
                    {
                        if (!context.DatabaseExists())
                        {
                            // Создание базы данных SimpleMembership без схемы миграции Entity Framework
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Не удалось инициализировать базу данных ASP.NET Simple Membership. Чтобы получить дополнительные сведения, перейдите по адресу: http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }*/
            }
        }
    }
}
