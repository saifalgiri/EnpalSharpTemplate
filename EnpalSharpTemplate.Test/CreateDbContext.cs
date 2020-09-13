//using EnpalSharpTemplate.Model;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace EnpalSharpTemplate.Test
//{
//    public class CreateDbContext : IDbContext
//    {
//        private readonly string _connectionString;
//        public CreateDbContext(string connectionString)
//        {
//            _connectionString = connectionString;
//        }
//        public Microsoft.EntityFrameworkCore.DbSet<PayloadModel> Blogs { get; set; }
//        IQueryable<T> IDbContext.Set<T>()
//        {
//            return base.Set<T>();
//        }
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            optionsBuilder.UseSqlServer(_connectionString);
//            base.OnConfiguring(optionsBuilder);
//        }
//        public void Rollback()
//        {
//            ChangeTracker.Entries().ToList().ForEach(x =>
//            {
//                x.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
//                var keys = GetEntityKey(x.Entity);
//                Set(x.Entity.GetType(), keys);
//            });
//        }

//        public System.Data.EntityState GetEntityState(object entity)
//        {
//            throw new NotImplementedException();
//        }

//        public System.Data.Metadata.Edm.MetadataWorkspace GetMetadata()
//        {
//            throw new NotImplementedException();
//        }

//        IDbSet<TEntity> IDbContext.Set<TEntity>()
//        {
//            throw new NotImplementedException();
//        }

//        public void SetEntityState(object entity, System.Data.EntityState state)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}