using Retailer.Data.Models.Learn;
using System;

namespace Retailer.DataAccess.DapperDemo
{
    public class PeopleData : IPeopleData
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public PeopleData(ISqlDataAccess sqlDataAccess)
        {
            this.sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<Person>> GetAllPeople()
        {
            string sqlQuery = "select * from person";

            return await sqlDataAccess.LoadData<Person, dynamic>(sqlQuery, new { });
        }

        public async Task SavePerson(Person person)
        {
            string sqlQuery = @"insert into person(FirstName, LastName, EmailAddress)
                            values(@FirstName,@LastName,@EmailAddress)";

            await sqlDataAccess.SaveData<dynamic>(sqlQuery, person);
        }
    }
}
