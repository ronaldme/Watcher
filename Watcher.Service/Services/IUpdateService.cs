﻿using System.Threading.Tasks;

namespace Watcher.Service.Services
{
    public interface IUpdateService
    {
        Task Start();
        Task Stop();
    }
}