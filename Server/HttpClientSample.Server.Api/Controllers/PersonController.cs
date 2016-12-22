using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;
using HttpClientSample.Server.Api.Models;
using HttpClientSample.Server.Api.Service;

namespace HttpClientSample.Server.Api.Controllers
{
    public class PersonController : ApiController
    {
        private readonly static Lazy<IList<Person>> _people = new Lazy<IList<Person>>(() => new PersonService().GeneratePeople(20), LazyThreadSafetyMode.PublicationOnly);
        private readonly object _sync = new object();
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
            var person = value;

            lock (_sync)
            {
                var foundPerson = _people.Value.FirstOrDefault(p => p.Id == person.Id) ?? new Person();

                if (foundPerson.Id == 0)
                {
                    foundPerson.Id = (_people.Value.Last().Id + 1);
                    _people.Value.Add(foundPerson);
                }

                foundPerson.FirstName = person.FirstName;
                foundPerson.LastName = person.LastName;
                foundPerson.Birthday = person.Birthday;

                _people.Value.Add(value);
            }

            return CreatedAtRoute("DefaultApi", new { id = _people.Value.IndexOf(value) }, _people.Value.Last());
        }

        // PUT: api/Person/5
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]Person value)
        {
            if (id < 0 || id >= _people.Value.Count()) return BadRequest();

            var foundPerson = _people.Value.FirstOrDefault(p => p.Id == value.Id);
            if(foundPerson == null) return NotFound();

            foundPerson.FirstName = value.FirstName;
            foundPerson.LastName = value.LastName;
            foundPerson.Birthday = value.Birthday;

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

            var people = firstNames.Zip(lastNames,
                (firstName, lastName) => new Person { FirstName = firstName, LastName = lastName }).ToList();

            foreach (var person in people)
            {
                person.Id = people.IndexOf(person) + 1;
                person.Birthday = RandomDayFunc();
            }

            return people;
        }

        private DateTime RandomDayFunc()
        {
            DateTime start = new DateTime(1965, 1, 1);
            Random gen = new Random();
            int range = ((TimeSpan)(new DateTime(1996, 1, 1) - start)).Days;
            return start.AddDays(gen.Next(range));
        }
    }
}
