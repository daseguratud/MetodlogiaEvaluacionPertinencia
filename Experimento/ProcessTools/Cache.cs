using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifaeTools
{
    public class Cache<T> where T:ICacheable, new()
    {
        public string CacheFileName {  get; set; }
        public char RS => '\x1E';//Separador de filas
        public char FS => '\x1F';//Separador de campos
        public List<T> StoredCache {  get; set; }
        public Cache(string CacheFileName){ 
            this.CacheFileName = CacheFileName;
            this.StoredCache = new List<T>();
            Load();
        }
        public void Load()
        {
            if (!File.Exists(CacheFileName)) {
                File.Create(CacheFileName).Close();
            }
            var cacheText = File.ReadAllText(CacheFileName);
            var cacheRows = cacheText.Split(RS,StringSplitOptions.RemoveEmptyEntries);
            foreach (var row in cacheRows) { 
                var fields = row.Split(FS);
                var newObject = new T();
                newObject.FromStringArray(fields);
                StoredCache.Add(newObject);
            }
        }
        public void Add(T newRegister) {
            File.AppendAllText(
                CacheFileName,
                $"{string.Join(FS, newRegister.ToStringArray())}{RS}");
            StoredCache.Add(newRegister);
        }
    }
}
