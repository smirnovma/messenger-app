using MessengerApp.Data.Context;
using MessengerApp.Entities.Chat;
using MessengerApp.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp.Data.SqlRepository
{
    public class SessionKeyRepository : IRepository<SessionKeyEntity>
    {
        private SqlDbContext db;

        public SessionKeyRepository()
        {
            db = new SqlDbContext();
        }

        public SessionKeyRepository(SqlDbContext sqlDbContext)
        {
            db = sqlDbContext;
        }

        public void Create(IEnumerable<SessionKeyEntity> items)
        {
            foreach (var item in items)
            {
                db.SessionKey.Add(item);
            }
        }

        public void Create(SessionKeyEntity item)
        {
            db.SessionKey.Add(item);
        }

        public void Delete(int id)
        {
            SessionKeyEntity sessionKeyEntity = db.SessionKey.Find(id);
            if (sessionKeyEntity != null)
            {
                db.SessionKey.Remove(sessionKeyEntity);
            }
        }

        public IQueryable<SessionKeyEntity> GetEntities()
        {
            return db.SessionKey;
        }

        public SessionKeyEntity GetEntity(int id)
        {
            return db.SessionKey.Find(id);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(SessionKeyEntity item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
