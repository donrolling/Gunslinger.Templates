using Common.BaseClasses;
using Gunslinger.Interfaces;
using Gunslinger.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace Gunslinger.Engines
{
    public class FileOutputEngine : LoggingWorker, ITemplateOutputEngine
    {
        private readonly List<string> _alreadyCleanedDirectories = new List<string>();

        public FileOutputEngine(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public OperationResult Write(string path, string output)
        {
            try
            {
                // prepare destination directory - todo: does this work if several directories need to be created?
                var destinationDirectory = path.Substring(0, path.LastIndexOf('\\'));
                Directory.CreateDirectory(destinationDirectory);
                // write it
                File.WriteAllText(path, output);
            }
            catch (Exception ex)
            {
                return OperationResult.Fail(ex.Message);
            }
            return OperationResult.Ok();
        }

        public OperationResult Write(string destinationPath, string entityName, string schema, string output)
        {
            var path = destinationPath.Replace("{entityName}", entityName).Replace("{schema}", schema);
            return Write(path, output);
        }

        public OperationResult CleanupOutputDirectory(string contextTemplateDirectory)
        {
            if (_alreadyCleanedDirectories.Contains(contextTemplateDirectory))
            {
                return OperationResult.Ok();
            }
            _alreadyCleanedDirectories.Add(contextTemplateDirectory);

            var di = new DirectoryInfo(contextTemplateDirectory);
            try
            {
                foreach (var file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (var dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception e)
            {
                return OperationResult.Fail($"Error FileOutputEngine.CleanupOutputDirectory: {e.Message}");
                throw;
            }
            return OperationResult.Ok();
        }
    }
}