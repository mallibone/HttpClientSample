using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpClientSample.Core;
using HttpClientSample.Core.Services;
using HttpClientSample.Core.Services.Impl;
using Xunit;

namespace HttpClientSample.IntegrationTest
{
    public class PersonServiceTest
    {
        private readonly PersonService _personService;

        public PersonServiceTest()
        {
            _personService = new PersonService();
        }

        [Fact]
        public async Task GetPeople_WhenInvoked_ReturnsAListOfPeople()
        {
            var people = await _personService.GetPeople();

            Assert.True(people.Any());
        }

        #region Create Perosn Tests
        [Fact]
        public async Task CreatePerson_WhenInvokedWithAvalidPerson_ItReturnsTrue()
        {
            var people = await _personService.GetPeople();

            var newPerson = new Person {FirstName = "Amy", LastName = "Pond"};

            var result = await _personService.CreatePerson(newPerson);

            Assert.True(result);
        }

        [Fact]
        public async Task CreatePerson_WhenInvokedWithAValidPerson_APersonIsAddedToTheList()
        {
            var people = await _personService.GetPeople();

            var newPerson = new Person{FirstName = "Amy", LastName = "Pond"};

            await _personService.CreatePerson(newPerson);

            var updatedPeople = await _personService.GetPeople();

            Assert.True(people.Count() < updatedPeople.Count());
        }
#endregion

        [Fact]
        public async Task UpdatePerson_WhenInvokedWithAValidPerson_ThePersonsNameIsUpdated()
        {
            var people = (await _personService.GetPeople()).ToList();

            var person = people[8];

            var firstName = person.FirstName.Equals("River") ? "Rose" : "River";
            var lastName = person.LastName.Equals("Song") ? "Tyler" : "Song";

            person.FirstName = firstName;
            person.LastName = lastName;

            await _personService.UpdatePerson(person);

            var servicePersonUpdated = (await _personService.GetPeople()).ToList()[8];
            Assert.True(servicePersonUpdated.FirstName == firstName);
            Assert.True(servicePersonUpdated.LastName == lastName);
        }
    }
}
