using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedParser.Core.Models
{
    public record Article
    {
        public string Header { get; set; }

        public string Link { get; set; }
    }
}
