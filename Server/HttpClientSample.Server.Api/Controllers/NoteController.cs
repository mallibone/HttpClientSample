using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;
using HttpClientSample.Server.Api.Models;

namespace HttpClientSample.Server.Api.Controllers
{
    public class NoteController : ApiController
    {
        private readonly static Lazy<IList<Note>> _note = new Lazy<IList<Note>>(() => new NoteService().GenerateNotes(20), LazyThreadSafetyMode.PublicationOnly);
        private readonly object _sync = new object();

        public IEnumerable<Note> Get()
        {
            return _note.Value;
        }

        // GET: api/Person/5
        public Note Get(int id)
        {
            if (id < 0 || _note.Value.Count < id) return null;
            return _note.Value[id];
        }

        // POST: api/Person
        [HttpPost]
        public IHttpActionResult Post(Note value)
        {
            if (value == null) return BadRequest();
            var note = value;
            Note noteFound;

            lock (_sync)
            {
                noteFound = _note.Value.FirstOrDefault(p => p.Id == note.Id) ?? new Note();

                if (note.Id == 0)
                {
                    noteFound.Id = (_note.Value.Last().Id + 1);
                    //_note.Value.Add(note);
                }

                noteFound.Title = note.Title;
                noteFound.Text = note.Text;
                noteFound.LastEdited = note.LastEdited;
                _note.Value.Add(noteFound);
            }

            return CreatedAtRoute("DefaultApi", new { id = _note.Value.IndexOf(noteFound) }, noteFound);
        }

        // PUT: api/Person/5
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]Note value)
        {
            if (id < 0 || id >= _note.Value.Count()) return BadRequest();

            var foundNot = _note.Value.FirstOrDefault(p => p.Id == value.Id);
            if(foundNot == null) return NotFound();

            foundNot.Title = value.Title;
            foundNot.Text = value.Text;
            foundNot.LastEdited = value.LastEdited;

            return Ok(value);
        }

        // DELETE: api/Person/5
        public IHttpActionResult Delete(int id)
        {
            if (id < 0 || _note.Value.All(n => n.Id != id)) return BadRequest();

            _note.Value.Remove(_note.Value.First(n => n.Id == id));

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }

    internal class NoteService
    {
        public IList<Note> GenerateNotes(int noteCount)
        {
            var notes = new List<Note>(noteCount);

            for (int i = 0; i < noteCount; ++i)
            {
                notes.Add(new Note()
                {
                    Id = i + 1,
                    Title = $"Note {i+1}",
                    Text = "Some lorem ipsum stuff and thing you now. They are going to love it",
                    LastEdited = RandomDayFunc()
                });
            }

            return notes;
        }

        private DateTime RandomDayFunc()
        {
            DateTime start = new DateTime(DateTime.Now.Year, 1, 1);
            Random gen = new Random();
            int range = ((TimeSpan)(new DateTime(DateTime.Now.Year, 12, 31) - start)).Days;
            return start.AddDays(gen.Next(range));
        }
    }
}