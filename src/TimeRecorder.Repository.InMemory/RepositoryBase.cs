using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain;
using TimeRecorder.Domain.Domain.Shared;

namespace TimeRecorder.Repository.InMemory
{
    public abstract class RepositoryBase<T, I> where T : Entity<T>, IIdentifiable<I>
    {
        private readonly List<T> _Collection = new List<T>();

        public void Add(T item)
        {
            _Collection.Add(item);
        }
           
        public void Edit(T item)
        {
            var index = _Collection.IndexOf(item);
            if (index < 0)
                throw new InvalidOperationException("編集対象の項目がみつかりませんでした");

            _Collection.RemoveAt(index);
            _Collection.Insert(index, item);
        }

        public void Delete(I id)
        {
            _Collection.RemoveAll(c => c.IsMatch(id));
        }

        public T[] Select() => _Collection.ToArray();
    }
}
