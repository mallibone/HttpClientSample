using System.Collections.Generic;
using System.Threading.Tasks;

namespace HttpClientSample.Core.Services
{
    public interface IPersonService
    {
        Task<bool> CreatePerson(Person person);
        Task<IEnumerable<Person>> GetPeople();
        Task<bool> UpdatePerson(Person person);
    }
}