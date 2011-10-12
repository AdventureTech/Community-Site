using System.Linq;

namespace CommunitySite.Core.Data
{
    public interface Repository
    {
        IQueryable<T> All<T>();
        void Save<T>(T item);
    }
}
