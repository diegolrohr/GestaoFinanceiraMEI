using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Fly01.Core.BL
{
    public class AppDataContextBase : DbContext
    {
        private Guid _empresaId;
        public Guid EmpresaId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_empresaId.ToString()))
                    throw new ApplicationException("ERRO! EmpresaId não informado.");

                return _empresaId;
            }
            set
            {
                _empresaId = value;
            }
        }

        private string _appUser;
        public string AppUser
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_appUser))
                    throw new ApplicationException("ERRO! AppUser não informado.");

                return _appUser;
            }
            set
            {
                _appUser = value;
            }
        }

        public AppDataContextBase(string conn) : base(conn)
        {
            //Configuration.UseDatabaseNullSemantics = true;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        //public LogRecordEvent GetAuditRecordsForChange(DbEntityEntry dbEntry)
        //{
        //    TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault() as TableAttribute;
        //    string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

        //    var keyNames = dbEntry.Entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).ToList();
        //    string keyName = keyNames[0].Name;

        //    LogRecordEvent result = new LogRecordEvent() { PlatformId = PlataformaUrl, Username = AppUser, TableName = tableName, RecordId = dbEntry.CurrentValues.GetValue<object>(keyName).ToString() };
        //    switch (dbEntry.State)
        //    {
        //        case EntityState.Added:
        //            {
        //                result.EventType = "I";

        //                IDictionary<string, object> newValues = new ExpandoObject();
        //                foreach (string propertyName in dbEntry.CurrentValues.PropertyNames)
        //                {
        //                    if (dbEntry.CurrentValues.GetValue<object>(propertyName) != null)
        //                    {
        //                        object value = dbEntry.CurrentValues.GetValue<object>(propertyName);

        //                        if (dbEntry.Entity.GetType().GetProperty(propertyName).PropertyType == typeof(Guid))
        //                            value = dbEntry.CurrentValues.GetValue<object>(propertyName).ToString();

        //                        newValues.Add(propertyName, value);
        //                    }
        //                }
        //                result.NewValues = newValues;

        //                break;
        //            }
        //        case EntityState.Modified:
        //            {
        //                result.EventType = "U";

        //                IDictionary<string, object> newValues = new ExpandoObject();
        //                IDictionary<string, object> originalValues = new ExpandoObject();
        //                foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
        //                {
        //                    if (!Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
        //                    {
        //                        object oValue = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName);
        //                        object nValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName);

        //                        if (dbEntry.Entity.GetType().GetProperty(propertyName).PropertyType == typeof(Guid))
        //                        {
        //                            oValue = oValue == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString();
        //                            nValue = nValue == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString();
        //                        }

        //                        originalValues.Add(propertyName, oValue);
        //                        newValues.Add(propertyName, nValue);
        //                    }
        //                }
        //                result.OriginalValues = originalValues;
        //                result.NewValues = newValues;

        //                break;
        //            }
        //    }

        //    return result;
        //}

        public async new Task<int> SaveChanges()
        {
            //var logDocuments = new List<LogRecordEvent>();
            //foreach (var ent in ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Modified))
            //    logDocuments.Add(GetAuditRecordsForChange(ent));

            var recordSaved = await base.SaveChangesAsync();
            //if (recordSaved > 0)
            //{
            //    await Task.Factory.StartNew(async () =>
            //    {
            //        var mongoHelper = new LogMongoHelper<LogRecordEvent>(ConfigurationManager.AppSettings["MongoDBLog"]);
            //        var collection = mongoHelper.GetCollection(ConfigurationManager.AppSettings["MongoCollectionNameLog"]);

            //        if (logDocuments.Count > 0)
            //            await collection.InsertManyAsync(logDocuments);
            //    });
            //}

            return recordSaved;
        }
    }
}