﻿using Watcher.Messages.Show;

namespace Watcher.Backend.Domain
{
    public class CurrentNextSeason
    {
        public Season Current { get; set; }
        public Season Next { get; set; }
    }
}