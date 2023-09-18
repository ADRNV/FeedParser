﻿using FeedParser.Core;
using FeedParser.Core.Models;

namespace FeedParser.Parsers.Handlers
{
    public class UpdateHandler : IUpdateHandler<IEnumerable<Article>>
    {
        public UpdateHandler()
        {
            
        }

        public Task OnUpdate(IEnumerable<Article> update)
        {
            return Task.CompletedTask;
        }
    }
}
