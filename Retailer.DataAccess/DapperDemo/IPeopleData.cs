using Retailer.Data.Models.Learn;
using System.Threading.Tasks;

namespace Retailer.DataAccess.DapperDemo
{
    public interface IPeopleData
    {
        Task<List<Person>> GetAllPeople();
        Task SavePerson(Person person);
    }
}