using OracleProvider.OracleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OracleProvider.Repository
{
    public interface IOraBaseRepository<T> where T : OracleBaseEntity
    {
        T Insert(T dto);

        IEnumerable<T> Insert(IEnumerable<T> list);

        T Update(T dto);

        bool Update(List<T> dtoList);

        T GetById(Guid id);

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

        T SingleBy(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetRange(int pageIndex, int pageSize);
        IEnumerable<T> GetRange(int pageIndex, int pageSize, out int count);


        IEnumerable<T> GetRange(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize);
        IEnumerable<T> GetRange(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize, out int count);
        T Delete(T dto);
    }
}
