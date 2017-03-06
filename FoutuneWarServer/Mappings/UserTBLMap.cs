using FluentNHibernate.Mapping;
using FoutuneWarServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoutuneWarServer.Mappings
{
    public class UserTBLMap : ClassMap<UserTBL>
    {
        public UserTBLMap()
        {
            Id(x => x.uid);
            Map(x => x.nickname);
            Map(x => x.heroIdx1);
            Map(x => x.heroIdx2);
            Map(x => x.heroIdx3);
            Map(x => x.heroIdx4);

            Table("usertbl");
        }
    }
}
