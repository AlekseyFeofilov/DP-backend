using DP_backend.Common;
using DP_backend.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_backend.Domain.Employment
{
    public class Group : BaseEntity
    {
       public int Number {  get; set; }
       public Grade Grade { get; set; }

    }
}
