using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTL.DTO
{
    public abstract class BaseDTO
    {
        [DefaultValue(0)]
        public int Id { get; set; }
    }
}
