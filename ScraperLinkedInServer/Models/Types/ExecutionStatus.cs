﻿namespace ScraperLinkedInServer.Models.Types
{
    public enum ExecutionStatus : byte
    {
        Created = 0,
        Queued = 1,
        Success = 2,
        Failed = 3,
        Any = 99
    }
}