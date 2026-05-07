using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifaeTools
{
    public interface ICacheable
    {
        /// <summary>
        /// Inicialice the instance from string array
        /// </summary>
        /// <param name="textFields">Fields in order like was stored</param>
        void FromStringArray(string[] textFields);
        /// <summary>
        /// Return the fields to be chached in the same order to restore
        /// </summary>
        string[] ToStringArray();
    }
}
