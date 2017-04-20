using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace HttpClientSample.IntegrationTest
{
    public class NoteServiceTest
    {
        private string _baseUri = "https://xamarinwebservice.azurewebsites.net/api/note";

        public NoteServiceTest()
        {
            
        }

        [Fact]
        public async Task GetNotes_ReturnsAtLeast20Items()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(_baseUri);
            Assert.True(response.IsSuccessStatusCode);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<IEnumerable<Note>>(jsonResponse);
            Assert.True(notes.Count() >= 20);
        }

        [Fact]
        public async Task AddNote_ItWillBeAddedToTheServersList()
        {
            var httpClient = new HttpClient();
            var timeStamp = DateTime.Now;
            var note = new Note {Title = "Hello Doctor", Text = "Geronimo", LastEdited = timeStamp};
            var jsonNote = JsonConvert.SerializeObject(note);
            var content = new StringContent(jsonNote, Encoding.UTF8, "application/json");

            var postResponse = await httpClient.PostAsync(_baseUri, content);
            Assert.True(postResponse.IsSuccessStatusCode);

            var response = await httpClient.GetAsync(_baseUri);
            Assert.True(response.IsSuccessStatusCode);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<IList<Note>>(jsonResponse);
            Assert.True(notes.Count() >= 20);
            Assert.True(notes.Any(n => n.LastEdited == timeStamp));
        }

        [Fact]
        public async Task AddAndRemoveNote_ItWillBeNoLongerBeRetrievableFromTheServer()
        {
            var httpClient = new HttpClient();
            var timeStamp = DateTime.Now;
            var noteText = "Geronimo 42";
            var note = new Note { Title = "Hello Doctor", Text = noteText, LastEdited = timeStamp };
            var jsonNote = JsonConvert.SerializeObject(note);
            var content = new StringContent(jsonNote, Encoding.UTF8, "application/json");

            var postResponse = await httpClient.PostAsync(_baseUri, content);
            Assert.True(postResponse.IsSuccessStatusCode);
            var noteJson = await postResponse.Content.ReadAsStringAsync();
            var noteToDelete = JsonConvert.DeserializeObject<Note>(noteJson);
            var deleteResponse = await httpClient.DeleteAsync(_baseUri+$"/{noteToDelete.Id}");
            Assert.True(deleteResponse.IsSuccessStatusCode);

            var response = await httpClient.GetAsync(_baseUri);
            Assert.True(response.IsSuccessStatusCode);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<IList<Note>>(jsonResponse);
            Assert.False(notes.Any(n => n.LastEdited == timeStamp));
        }

        [Fact]
        public async Task EditNote_TheChangesAreReadFromTheServer()
        {
            const int id = 8;
            var httpClient = new HttpClient();
            var note = await GetNote(httpClient, id);

            var timeStamp = DateTime.Now;
            var noteText = $"Geronimo 42 {timeStamp.ToString("O")}";
            note.Text = noteText;
            note.LastEdited = timeStamp;
            var jsonNote = JsonConvert.SerializeObject(note);
            var content = new StringContent(jsonNote, Encoding.UTF8, "application/json");
            var putResponse = await httpClient.PutAsync(_baseUri + $"/{id}", content);
            Assert.True(putResponse.IsSuccessStatusCode);

            var updatedNote = await GetNote(httpClient, id);

            Assert.Equal(noteText, updatedNote.Text);
            Assert.Equal(timeStamp, updatedNote.LastEdited);
        }

        private async Task<Note> GetNote(HttpClient httpClient, int id)
        {
            var response = await httpClient.GetAsync(_baseUri + $"/{id}");
            Assert.True(response.IsSuccessStatusCode);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var note = JsonConvert.DeserializeObject<Note>(jsonResponse);
            return note;
        }
    }

    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime LastEdited { get; set; }
    }
}