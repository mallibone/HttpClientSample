using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpClientSample.Server.Api.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime LastEdited { get; set; }
    }
}