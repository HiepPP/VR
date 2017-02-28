using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VR.Infrastructure.Utilities;
using VR.OracleProvider.OracleEntities;

namespace VR.OracleProvider.OracleRepository
{
    public class OraBaseRepository<T>: IOraBaseRepository<T> where T: OracleBaseEntity
    {
        public T Insert(T dto)
        {
            T entity = null;

            try
            {
                using (var db = new OracleContext())
                {
                    entity = db.Set<T>().Add(dto);
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
                entity = null;
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
                entity = null;
            }
            catch (Exception ex)
            {
                entity = null;
                Logger.Error(ex.Message, ex);
            }

            return entity;
        }
        public IEnumerable<T> Insert(IEnumerable<T> list)
        {
            using (var db = new OracleContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    IEnumerable<T> tList = null;
                    try
                    {
                        tList = db.Set<T>().AddRange(list);
                        db.SaveChanges();
                        trans.Commit();//Data Saved Successfully. Transaction Commited
                                       //tList = list;
                    }
                    catch (DbException ex)
                    {
                        Logger.Error(ex.Message, ex);
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();//Error Occured during data saved. Transaction Rolled Back
                        Logger.Error(ex.Message, ex);
                    }
                    return tList;
                }
            }
        }

        public T Update(T dto)
        {
            T entity = null;

            try
            {
                using (var db = new OracleContext())
                {
                    entity = db.Set<T>().Find(dto.ID);
                    if (entity != null)
                    {
                        db.Entry(entity).CurrentValues.SetValues(dto);
                        db.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
                entity = null;
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
                entity = null;
            }
            catch (Exception ex)
            {
                entity = null;
                Logger.Error(ex.Message, ex);
            }

            return entity;
        }

        public bool Update(List<T> dtoList)
        {
            using (var db = new OracleContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    T entity = null;
                    try
                    {

                        var dbSet = db.Set<T>();
                        dtoList.ForEach(x =>
                        {
                            entity = dbSet.Find(x.ID);
                            if (entity != null)
                            {
                                db.Entry(entity).CurrentValues.SetValues(x);
                            }
                        });

                        db.SaveChanges();
                        trans.Commit();//Data Saved Successfully. Transaction Commited
                        return true;

                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                            }
                        }
                        return false;
                    }
                    catch (DbException ex)
                    {
                        Logger.Error(ex.Message, ex);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();//Error Occured during data saved. Transaction Rolled Back
                        Logger.Error(ex.Message, ex);
                        return false;
                    }
                }
            }
        }

        public T GetById(int id)
        {
            T entity = null;

            try
            {
                using (var db = new OracleContext())
                {
                    entity = db.Set<T>().Find(id);
                }
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return entity;
        }

        //public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        //{
        //    IEnumerable<T> result = null;

        //    try
        //    {
        //        using (var db = new OracleContext())
        //        {
        //            result = db.Set<T>().Where(i => !i.IsDeleted).Where(predicate).ToList();
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
        //            }
        //        }
        //    }
        //    catch (DbException ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //    }

        //    return result ?? Enumerable.Empty<T>();
        //}

        public T SingleBy(Expression<Func<T, bool>> predicate)
        {
            T entity = null;

            try
            {
                using (var db = new OracleContext())
                {
                    entity = db.Set<T>().FirstOrDefault(predicate);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return entity;
        }

        public IEnumerable<T> GetAll()
        {
            IEnumerable<T> result = null;

            try
            {
                using (var db = new OracleContext())
                {
                    //var r = db.TRUCKs.ToList();
                    result = db.Set<T>().ToList();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return result ?? Enumerable.Empty<T>();
        }

        //public IEnumerable<T> GetRange(int pageIndex, int pageSize)
        //{
        //    IEnumerable<T> result = null;

        //    try
        //    {
        //        using (var db = new OracleContext())
        //        {
        //            result = db.Set<T>().Where(i => !i.IsDeleted).OrderBy(i => i.ID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
        //            }
        //        }
        //    }
        //    catch (DbException ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //    }

        //    return result ?? Enumerable.Empty<T>();
        //}
        //public IEnumerable<T> GetRange(int pageIndex, int pageSize, out int count)
        //{
        //    IEnumerable<T> result = null;
        //    count = 0;
        //    try
        //    {
        //        using (var db = new OracleContext())
        //        {
        //            var list = db.Set<T>().Where(i => !i.IsDeleted);
        //            count = list.Count();
        //            result = list.OrderBy(i => i.ID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
        //            }
        //        }
        //    }
        //    catch (DbException ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //    }

        //    return result ?? Enumerable.Empty<T>();
        //}


        //public IEnumerable<T> GetRange(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize)
        //{
        //    IEnumerable<T> result = null;

        //    try
        //    {
        //        using (var db = new OracleContext())
        //        {
        //            result = db.Set<T>().Where(i => !i.IsDeleted).Where(predicate).OrderBy(i => i.ID).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
        //            }
        //        }
        //    }
        //    catch (DbException ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //    }

        //    return result ?? Enumerable.Empty<T>();
        //}
        //public IEnumerable<T> GetRange(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize, out int count)
        //{
        //    count = 0;
        //    IEnumerable<T> result = null;

        //    try
        //    {
        //        using (var db = new OracleContext())
        //        {
        //            var list = db.Set<T>().Where(i => !i.IsDeleted).Where(predicate).ToList();
        //            count = list.Count();
        //            result = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
        //            }
        //        }
        //    }
        //    catch (DbException ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //    }

        //    return result ?? Enumerable.Empty<T>();
        //}
        //public T Delete(T dto)
        //{
        //    T entity = null;

        //    try
        //    {
        //        using (var db = new OracleContext())
        //        {
        //            entity = db.Set<T>().Find(dto.ID);
        //            if (entity != null)
        //            {
        //                entity.IsDeleted = true;
        //                //entity = db.Set<T>().Remove(entity);
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
        //            }
        //        }
        //    }
        //    catch (DbException ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //        entity = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //    }

        //    return entity;
        //}

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> result = null;

            try
            {
                using (var db = new OracleContext())
                {
                    result = db.Set<T>().Where(predicate).ToList();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return result ?? Enumerable.Empty<T>();
        }
    }
}
