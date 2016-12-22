using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpClientSample.Core.Services.Impl
{
    public class PersonService : IPersonService
    {
        private const string BASE_URI = "https://httpclientsample.azurewebsites.net/api/person";
        private HttpClient _httpClient;

        public PersonService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<Person>> GetPeople()
        {
            var result = await _httpClient.GetAsync(BASE_URI);
            var peopleJson = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<Person>>(peopleJson);
        }

        public async Task<bool> CreatePerson(Person person)
        {
            var personJson = JsonConvert.SerializeObject(person);
            var content = new StringContent(personJson, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(BASE_URI, content);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePerson(Person person)
        {
            var uri = Path.Combine(BASE_URI, person.Id.ToString());

            var personJson = JsonConvert.SerializeObject(person);
            var content = new StringContent(personJson, Encoding.UTF8, "application/json");

            var result = await _httpClient.PutAsync(uri, content);

            return result.IsSuccessStatusCode;
        }
    }
}
