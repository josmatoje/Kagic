using System;
using System.Collections.Generic;
using System.Text;

namespace Kagic_BL
{
    public class DDL_BL
    {
        public static void createDatabase() 
        {
            Kagic_DAL.Database.DDL.createDatabase();
        }
    }
}
