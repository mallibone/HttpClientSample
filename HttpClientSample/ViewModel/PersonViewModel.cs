using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HttpClientSample.Core;
using HttpClientSample.Core.Services;

namespace HttpClientSample.ViewModel
{
    internal class PersonViewModel:ViewModelBase
    {
        private readonly IPersonService _personService;
        private int _id;
        private string _firstname;
        private string _lastname;

        public PersonViewModel(IPersonService personService)
        {
            if (personService == null) throw new ArgumentNullException(nameof(personService));
            _personService = personService;

            StoreCommand = new RelayCommand(StorePerson, () => HasPendingChanges);
        }

        public bool HasPendingChanges { get; set; }

        public string Firstname
        {
            get { return _firstname; }
            set
            {
                if (value == _firstname) return;
                _firstname = value;
                RaisePropertyChanged(nameof(Firstname));
            }
        }

        public string Lastname
        {
            get { return _lastname; }
            set
            {
                if (_lastname == value) return;
                _lastname = value;
                RaisePropertyChanged(nameof(Lastname));
            }
        }

        public ICommand StoreCommand { get; set; }

        public async Task Init(int id)
        {
            _id = id;
            var person = id > 0 ? (await _personService.GetPeople()).ToList()[id] : new Person("", "");
            Firstname = person.FirstName;
            Lastname = person.LastName;
        }

        private async void StorePerson()
        {
            HasPendingChanges = false;

            var person = new Person(Firstname, Lastname);

            if (_id > 0)
            {
                await _personService.UpdatePerson(_id, person);
            }
            else
            {
                await _personService.CreatePerson(person);
            }

        }
    }
}