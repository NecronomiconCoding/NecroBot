using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.Utils
{
    public class DependencyFactory
    {
        public delegate object Creator(DependencyFactory container);

        private readonly Dictionary<string, object> _configuration
                        = new Dictionary<string, object>();

        private readonly Dictionary<Type, Creator> _typeToCreator
                        = new Dictionary<Type, Creator>();

        public Dictionary<string, object> Configuration
        {
            get { return _configuration; }
        }

        public void Register<T>(Creator creator)
        {
            _typeToCreator.Add(typeof(T), creator);
        }

        public T Create<T>()
        {
            return (T)_typeToCreator[typeof(T)](this);
        }

        public T GetConfiguration<T>(string name)
        {
            return (T)_configuration[name];
        }
    }
}
