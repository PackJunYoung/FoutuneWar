using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoutuneWarServer.Entities
{
    public class UserTBL
    {
        public virtual int uid { get; set; }
        public virtual string nickname { get; set; }
        public virtual int heroIdx1 { get; set; }
        public virtual int heroIdx2 { get; set; }
        public virtual int heroIdx3 { get; set; }
        public virtual int heroIdx4 { get; set; }

        public UserTBL()
        {
        }

        public UserTBL(string name)
        {
            uid = heroIdx1 = heroIdx2 = heroIdx3 = heroIdx4 = 1;
            nickname = name;
        }
    }
}
