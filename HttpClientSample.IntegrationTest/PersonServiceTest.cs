using System;
using System.Collections.Generic;
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

            var newPerson = new Person("Amy", "Pond");

            var result = await _personService.CreatePerson(newPerson);

            Assert.True(result);
        }

        [Fact]
        public async Task CreatePerson_WhenInvokedWithAValidPerson_APersonIsAddedToTheList()
        {
            var people = await _personService.GetPeople();

            var newPerson = new Person("Amy", "Pond");

            await _personService.CreatePerson(newPerson);

            var updatedPeople = await _personService.GetPeople();

            Assert.True(people.Count() < updatedPeople.Count());
        }
#endregion

        [Fact]
        public async Task UpdatePerson_WhenInvokedWithAValidPerson_ThePersonsNameIsUpdated()
        {
            var people = (await _personService.GetPeople()).ToList();

            var person = people[42];

            var firstName = person.FirstName == "River" ? "Rose" : "River";
            var lastName = person.LastName == "Song" ? "Tyler" : "Song";

            var updatedPerson = new Person(firstName, lastName);

            await _personService.UpdatePerson(42, updatedPerson);

            var servicePersonUpdated = (await _personService.GetPeople()).ToList()[42];
            Assert.True(servicePersonUpdated.FirstName == firstName);
            Assert.True(servicePersonUpdated.LastName == lastName);
        }
    }
}
