using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BTravel.CommonDefinitions.Enums;
using BTravel.DAL;
using Microsoft.EntityFrameworkCore;

namespace BTravel.BL.Services
{
    public class BaseService
    {
        protected const string IDColumn = "Id";
        protected const string OrderByCommand = "OrderBy";
        protected const string OrderByDescCommand = "OrderByDescending";

        protected static IQueryable<Q> OrderByDynamic<Q>(IQueryable<Q> query, string orderByColumn, bool isDesc)
        {
            var QType = typeof(Q);
            // Dynamically creates a call like this: query.OrderBy(p => p.SortColumn)
            var parameter = Expression.Parameter(QType, "p");
            Expression resultExpression = null;
            var property = QType.GetProperty(orderByColumn ?? IDColumn);
            // this is the part p.SortColumn
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            // this is the part p => p.SortColumn
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // finally, call the "OrderBy" / "OrderByDescending" method with the order by lamba expression
            resultExpression = Expression.Call(typeof(Queryable), isDesc ? OrderByDescCommand : OrderByCommand,
                new[] { QType, property.PropertyType }, query.Expression, Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<Q>(resultExpression);
        }

        protected static IQueryable<Q> ApplyPaging<Q>(IQueryable<Q> query, int pageSize, int pageIndex)
        {
            pageSize = pageSize == 0 ? CommonDefinitions.Constants.defaultPageSize : pageSize;
            var skipedPages = pageSize * pageIndex;

            query = query.Skip(skipedPages).Take(pageSize);
            return query;
        }

        protected static IEnumerable<Q> ApplyPaging<Q>(IEnumerable<Q> query, int pageSize, int pageIndex)
        {
            pageSize = pageSize == 0 ? CommonDefinitions.Constants.defaultPageSize : pageSize;
            var skipedPages = pageSize * pageIndex;

            query = query.Skip(skipedPages).Take(pageSize);
            return query;
        }

        public static bool CheckRoleAccessability(int roleID, string serviceName, BTravelDbContext context = null, string connectionString = null)
        {
            if (context == null)
                context = new BTravelDbContext(GetDBContextConnectionOptions(connectionString));

            if (roleID == 0)
                return false;

            if (roleID == (long)ERole.Admin)
                return true;

            var service = context.AppServices.FirstOrDefault(c => !c.IsDeleted && c.Name.ToLower() == serviceName.ToLower());
            if (service == null)
                return false;

            var canAccess = false;

            canAccess = context.RoleAppServices.Any(c => c.RoleId == roleID && c.AppServiceId == service.AppServiceId && c.IsActive && !c.IsDeleted);

            return canAccess;
        }

        public static DbContextOptions<BTravelDbContext> GetDBContextConnectionOptions(string connectionString)
        {
            return MySqlDbContextOptionsBuilderExtensions.UseMySql(new DbContextOptionsBuilder<BTravelDbContext>(), connectionString, ServerVersion.AutoDetect(connectionString), null).Options;
        }
    }
}