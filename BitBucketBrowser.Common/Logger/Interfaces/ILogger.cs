﻿namespace BitBucketBrowser.Common.Logger.Interfaces
{
    internal interface ILogger
    {
        void Trace(string message);

        void Debug(string message);

        void Info(string message);

        void Warn(string message);

        void Error(string message);

        void Fatal(string message);
    }
}