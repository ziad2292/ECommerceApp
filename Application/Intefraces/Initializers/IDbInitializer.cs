using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intefraces.Initializers
{
    public interface IDbInitializer
    {
        Task InitializeDbAsync();

    }
}
