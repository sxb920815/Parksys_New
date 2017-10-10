using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MCLYGV3.DB
{
    public class BaseDataService<T> where T : class, new()
    {
        /// <summary>
        /// 上下文网关
        /// </summary>
        protected DBContext db = new DBContext();

        /// <summary>
        /// 根据Id查询单条数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T GetSingleById(int Id)
        {

            var model = new T();
            model = db.Set<T>().Find(new object[] { Id });
            return model;
        }

        /// <summary>
        /// 根据条件查询单挑数据
        /// </summary>
        /// <param name="whereLambds"></param>
        /// <returns></returns>
        public T GetSingleCondition(Expression<Func<T, bool>> whereLambds)
        {
            var model = db.Set<T>().FirstOrDefault(whereLambds);
            return model;
        }

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Create(T model)
        {
            db.Entry<T>(model).State = EntityState.Added;
            return db.SaveChanges() > 0;
        }

        /// <summary>
        /// 同时增加多条数据到一张表（事务处理）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool CreateList(List<T> entitys)
        {
            foreach (var entity in entitys)
            {
                db.Entry<T>(entity).State = EntityState.Added;
            }
            // entitys.ForEach(c=>db.Entry<T>(c).State = EntityState.Added);//等价于上面的循环
            return db.SaveChanges() > 0;
        }

        /// <summary>
        /// 修改一条数据，会修改所有列的值，没有赋值的属性将会被赋予属性类型的默认值**************
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Modified;//将所有属性标记为修改状态
            return db.SaveChanges() > 0;
        }

        /// <summary>
        /// 修改一条数据,会修改指定列的值
        /// </summary>
        /// <param name="entity">要修改的实体对象</param>
        /// <param name="proNames">要修改的属性名称</param>
        /// <returns></returns>
        public bool Update(T entity, params string[] proNames)
        {
            db.Set<T>().Attach(entity);
            DbEntityEntry<T> dbee = db.Entry<T>(entity);
            dbee.State = EntityState.Unchanged;//先将所有属性状态标记为未修改
            proNames.ToList().ForEach(c => dbee.Property(c).IsModified = true);//将要修改的属性状态标记为修改
            return db.SaveChanges() > 0;
        }

        /// <summary>
        /// 删除一个实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(T entity)
        {
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Deleted;
            return db.SaveChanges() > 0;
        }

        /// <summary>
        /// 根据Id删除一个实体对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteById(int Id)
        {
            var entity = GetSingleById(Id);
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Deleted;
            return db.SaveChanges() > 0;
        }

        /// <summary>
        /// 根据条件批量删除实体对象
        /// </summary>
        /// <param name="whereLambds"></param>
        /// <returns></returns>
        public bool DeleteEntityByWhere(Expression<Func<T, bool>> whereLambds)
        {
            var data = db.Set<T>().Where<T>(whereLambds).ToList();
            return DeleteList(data);
        }

        /// <summary>
        /// 事务批量删除实体对象
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool DeleteList(List<T> entitys)
        {
            foreach (var item in entitys)
            {
                db.Set<T>().Attach(item);
                db.Entry<T>(item).State = EntityState.Deleted;
            }
            return db.SaveChanges() > 0;
        }

        //带条件查询
        public IList<T> GetList(Func<T, bool> whereLambds)
        {
            return db.Set<T>().Where<T>(whereLambds).ToList<T>();
        }

        //带排序查询
        public IList<T> GetList<S>(Expression<Func<T, bool>> whereLambds, bool isAsc, Expression<Func<T, S>> orderByLambds)
        {
            var temp = db.Set<T>().Where<T>(whereLambds);
            if (isAsc)
            {
                return temp.OrderBy<T, S>(orderByLambds).ToList<T>();
            }
            else
            {
                return temp.OrderByDescending<T, S>(orderByLambds).ToList<T>();
            }
        }


        //带分页查询
        public List<T> GetListByPaged(int pageIndex, int pageSize, out int rows, Expression<Func<T, bool>> whereLambds, bool isAsc, params OrderModelField[] orderByExpression)
        {
            var temp = db.Set<T>().Where<T>(whereLambds);
            rows = temp.Count();

            //创建表达式变量参数
            var parameter = Expression.Parameter(typeof(T), "o");

            if (orderByExpression != null && orderByExpression.Length > 0)
            {
                for (int i = 0; i < orderByExpression.Length; i++)
                {
                    //根据属性名获取属性
                    var property = typeof(T).GetProperty(orderByExpression[i].propertyName);
                    //创建一个访问属性的表达式
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);


                    string OrderName = orderByExpression[i].IsDESC ? "OrderByDescending" : "OrderBy";


                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), OrderName, new Type[] { typeof(T), property.PropertyType }, temp.Expression, Expression.Quote(orderByExp));
                    temp = temp.Provider.CreateQuery<T>(resultExp);
                }
            }

            temp = temp.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);
            return temp.ToList<T>();
        }


        public int Count(Expression<Func<T, bool>> whereLambds)
        {
            var temp = db.Set<T>().Where<T>(whereLambds);
            return temp.Count();
        }

    }

    public struct OrderModelField
    {
        public string propertyName { get; set; }
        public bool IsDESC { get; set; }
    }
}
