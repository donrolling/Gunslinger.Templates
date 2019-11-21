﻿using System;
using System.IO;
using Common.BaseClasses;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Microsoft.Extensions.Logging;

namespace Gunslinger.DataProviders
{
    public class FileTemplateProvider : LoggingWorker, IFileProvider
    {
        public FileTemplateProvider(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public string Get(GenerationContext context, string filename)
        {
            return Get($"{context.TemplateDirectory}\\{filename}");
        }

        public string Get(string fullPath)
        {
            try
            {
                return File.ReadAllText(fullPath);
            }
            catch (Exception e)
            {
                this.Logger.LogError($"FileTemplateProvider Get({fullPath})\r\nError: {e.Message}");
                return string.Empty;
            }
        }
    }
}