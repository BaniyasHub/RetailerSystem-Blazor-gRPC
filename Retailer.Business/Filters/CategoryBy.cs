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
    public static class CategoryBy
    {
        public static Expression<Func<Category, bool>> All()
        {
            return x => true;
        }

        public static Expression<Func<Category, bool>> CategoryIdList(List<int> categoryIds)
        {
            return x => categoryIds.Contains(x.Id);
        }

        public static Expression<Func<Category, bool>> Id(int categoryId)
        {
            return x => x.Id == categoryId;
        }


        public static Expression<Func<Category, bool>> CategoryIds(string categoryIds)
        {
            var ids =
                (from rid in categoryIds.Split('|')
                 where rid != ""
                 select Convert.ToInt32(rid))
                .ToList();
            return x => ids.Contains(x.Id);
        }


        public static Expression<Func<Category, bool>> NameAndCreationDate(string name, DateTime creationDate)
        {
            return x => x.Name == name && x.CreatedDate == creationDate;
        }


        public static Expression<Func<Category, bool>> NameAndId(string name, int id)
        {
            return Name(name).And(Id(id));
        }
       

        public static Expression<Func<Category, bool>> Name(string name)
        {
            return x => x.Name.ToLower() == name.ToLower();
        }


    }
}
