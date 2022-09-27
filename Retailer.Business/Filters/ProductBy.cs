using Retailer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Retailer.Common.Utility;

namespace Retailer.Business.Filters
{
    public static class ProductBy
    {
        public static Expression<Func<Product, bool>> All()
        {
            return x => true;
        }

        public static Expression<Func<Product, bool>> ProductIdList(List<int> productIds)
        {
            return x => productIds.Contains(x.Id);
        }

        public static Expression<Func<Product, bool>> Id(int productId)
        {
            return x => x.Id == productId;
        }


        public static Expression<Func<Product, bool>> ProductIds(string productIds)
        {
            var ids =
                (from rid in productIds.Split('|')
                 where rid != ""
                 select Convert.ToInt32(rid))
                .ToList();
            return x => ids.Contains(x.Id);
        }


        public static Expression<Func<Product, bool>> NameAndColor(string name, string color)
        {
            return x => x.Name == name && x.Color == color;
        }


        public static Expression<Func<Product, bool>> NameAndId(string name, int id)
        {
            return Name(name).And(Id(id));
        }


        public static Expression<Func<Product, bool>> Name(string name)
        {
            return x => x.Name.ToLower() == name.ToLower();
        }

        public static Expression<Func<Product, bool>> ProductIds(List<int> ids)
        {
            return x => ids.Contains(x.Id);
        }


    }
}
