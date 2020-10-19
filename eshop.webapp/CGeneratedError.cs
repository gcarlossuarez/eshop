using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop.webapp
{
    public class CGeneratedError
    {
        public short Id { get; set; }
        public string Description { get; set; }

        public CGeneratedError(short p_IdError, string p_DescriptionError)
        {
            this.Id = p_IdError;
            this.Description = p_DescriptionError;
        }
    }
}
