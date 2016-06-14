using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{

    public  class TestContext :DbContext
    {
        public TestContext(string conStr)
            : base(conStr)
        {
        }

        public  DbSet<General> Generals { get; set; }
        public  DbSet<threshold> thresholds { get; set; }
    }
}
