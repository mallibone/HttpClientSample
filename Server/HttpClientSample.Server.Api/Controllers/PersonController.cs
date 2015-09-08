using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using HttpClientSample.Server.Api.Models;
using HttpClientSample.Server.Api.Service;

namespace HttpClientSample.Server.Api.Controllers
{
    public class PersonController : ApiController
    {
        private readonly static Lazy<IList<Person>> _people = new Lazy<IList<Person>>(() => new PersonService().GeneratePeople(1000), LazyThreadSafetyMode.PublicationOnly);
        // GET: api/Person
        public IEnumerable<Person> Get()
        {
            return _people.Value;
        }

        // GET: api/Person/5
        public Person Get(int id)
        {
            if (id < 0 || _people.Value.Count < id) return null;
            return _people.Value[id];
        }

        // POST: api/Person
        [HttpPost]
        public IHttpActionResult Post(Person value)
        {
            if (value == null) return BadRequest();

            _people.Value.Add(value);

            return CreatedAtRoute("DefaultApi", new { id = _people.Value.IndexOf(value) }, _people.Value.Last());
        }

        // PUT: api/Person/5
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]Person value)
        {
            if (id < 0 || id >= _people.Value.Count()) return BadRequest();

            _people.Value[id] = value;

            return Ok(value);
        }

        // DELETE: api/Person/5
        public IHttpActionResult Delete(int id)
        {
            if (id < 0 || id >= _people.Value.Count()) return BadRequest();

            _people.Value.RemoveAt(id);

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }

    internal class PersonService
    {
        public IList<Person> GeneratePeople(int count)
        {
            var firstNames = new List<string>(count);
            var lastNames = new List<string>(count);

            for (int i = 0; i < count; ++i)
            {
                firstNames.Add(NameGenerator.GenRandomFirstName());
                lastNames.Add(NameGenerator.GenRandomLastName());
            }

            return firstNames.Zip(lastNames, (firstName, lastName) => new Person(firstName, lastName)).ToList();
        }
    }
}
