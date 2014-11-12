using System;
using SIKCIPayloadProcessor.Classes;
using SIKCIPayloadProcessor.DAL;
using SIKCIPayloadProcessor.Interfaces;
using XlnDependencyContainer;
using XLNLogger;

namespace SIKCIPayloadProcessor
{
    class Program
    {      
        static void Main(string[] args)
        {

            var logger = DependencyContainer.GetCurrent().Resolve<IXlnLogger>();
            try
            {
                logger.Info("Starting application");
                RegisterDependencies();

                var instanceController = DependencyContainer.GetCurrent().Resolve<IAppSingleController>();

                if (!instanceController.IsNewInstance)
                {
                    Console.WriteLine("Process is Running");
                    return;
                }


                var configurationService = DependencyContainer.GetCurrent().Resolve<IConfiguratonServices>();
                var fileService = DependencyContainer.GetCurrent().Resolve<IFileServices>();

                var loadFilesProcess = new LoadFileService(configurationService, fileService);

                loadFilesProcess.PerformLoadingFiles();

                var inboundPayloadFiles = loadFilesProcess.GetInBoundPayloadFiles();
                var destinationFolder = loadFilesProcess.GetDestinationFolder();
                var archivedFolder = loadFilesProcess.GetArchivedFolder();

                var processors = DependencyContainer.GetCurrent().ResolveAll<ISIPayloadProcessor>();

                foreach (var processor in processors)
                {
                    logger.Info(String.Format("Start processing with {0}", processor.GetType().Name));
                    processor.Run(inboundPayloadFiles, destinationFolder, archivedFolder);
                    logger.Info(String.Format("Stop processing with {0}", processor.GetType().Name));
                }
                logger.Info("Finishing processing");
            }
            catch (Exception ex)
            {
                logger.Fatal(String.Format("Error: {0}", ex.Message));
                logger.Fatal(String.Format("Stack Trace: {0}", ex.StackTrace));
            }                   
        }

        private static void RegisterDependencies()
        {
            var container = DependencyContainer.GetCurrent();
            container.Register<IAppSingleController>(typeof(AppSingleController));
            container.Register<IConfiguratonServices>(typeof(ConfigurationService));
            container.Register<IFileServices>(typeof(FileService));
            container.Register<ISIPayloadProcessor>(typeof(GetOrderMessageProcessor));
            container.Register<ISIPayloadProcessor>(typeof(GetFaultChangesProcessor));
            container.Register<ISIPayloadProcessor>(typeof(GetUnsolicitedCeaseProcessor));
            container.Register<ISiKciPayloadDataAccess>(typeof(SIKCIPayloadDataAccess));
        }
    }
}
