using Retailer.Common.Utility;
using Retailer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Business.Filters
{
    internal class CustomerBy
    {
        public static Expression<Func<Customer, bool>> All()
        {
            return x => true;
        }

        public static Expression<Func<Customer, bool>> CustomerIdList(List<int> customerIds)
        {
            return x => customerIds.Contains(x.Id);
        }

        public static Expression<Func<Customer, bool>> Id(int customerId)
        {
            return x => x.Id == customerId;
        }


        public static Expression<Func<Customer, bool>> CustomerIds(string customerIds)
        {
            var ids =
                (from rid in customerIds.Split('|')
                 where rid != ""
                 select Convert.ToInt32(rid))
                .ToList();
            return x => ids.Contains(x.Id);
        }



        public static Expression<Func<Customer, bool>> NameAndId(string name, int id)
        {
            return Name(name).And(Id(id));
        }


        public static Expression<Func<Customer, bool>> Name(string name)
        {
            return x => x.FirstName.ToLower() == name.ToLower();
        }
    }
}
