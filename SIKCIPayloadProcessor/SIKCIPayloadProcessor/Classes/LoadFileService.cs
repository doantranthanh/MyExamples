using System;
using System.Collections.Generic;
using System.IO;
using SIKCIPayloadProcessor.Enums;
using SIKCIPayloadProcessor.Interfaces;

namespace SIKCIPayloadProcessor.Classes
{
    public class LoadFileService 
    {
        public IConfiguratonServices ConfiguratonServices { get; set; }
        public IFileServices FileServices { get; set; }
        public Dictionary<string, string> SourcePath { get; set; }
        public Dictionary<string, string> DestinationPath { get; set; }
        public DirectoryInfo SourceInDirectoryInfo { get; set; }
        public DirectoryInfo SourceOutDirectoryInfo { get; set; }
        public FileInfo[] InBoundPayloadFiles { get; set; }
        public FileInfo[] OutBoundPayloadFiles { get; set; }

        public LoadFileService(IConfiguratonServices configuratonServices, IFileServices fileServices)
        {
            ConfiguratonServices = configuratonServices;
            FileServices = fileServices;
        }

        public void PerformLoadingFiles()
        {
            CreatePaths();
            CreateDirectoryInfo();

            if (FileServices.IsDirectoryExist(SourcePath[Source.SI_INBOUND.ToString()]))
            {
                InBoundPayloadFiles = LoadFiles(SourceInDirectoryInfo);
            }

            if (FileServices.IsDirectoryExist(SourcePath[Source.SI_OUTBOUND.ToString()]))
            {
                OutBoundPayloadFiles = LoadFiles(SourceOutDirectoryInfo);
            }
        }

        public FileInfo[] LoadFiles(DirectoryInfo source)
        {
            return FileServices.LoadFilesFromDirectory(source);
        }

        public void CreatePaths()
        {
            SourcePath = new Dictionary<string, string>();
            DestinationPath = new Dictionary<string, string>();

            SourcePath.Add(Source.SI_OUTBOUND.ToString(), ConfiguratonServices.GetPathValueFromConfig(Platform.SI.ToString(), PayloadDirectoryPath.SIPayloadDirectoryPath.SIRequestPayloadDirPath.ToString()));
            SourcePath.Add(Source.SI_INBOUND.ToString(), ConfiguratonServices.GetPathValueFromConfig(Platform.SI.ToString(), PayloadDirectoryPath.SIPayloadDirectoryPath.SIResponsePayloadDirPath.ToString()));

            DestinationPath.Add(Source.SI_INBOUND.ToString(), String.Format(@"{0}\{1}", ConfiguratonServices.GetPathValueFromConfig(Platform.SI.ToString(), PayloadDirectoryPath.SIPayloadDirectoryPath.ArchiveDestinationPath.ToString()), PayloadDirection.IN));
            DestinationPath.Add(Source.SI_OUTBOUND.ToString(), String.Format(@"{0}\{1}", ConfiguratonServices.GetPathValueFromConfig(Platform.SI.ToString(), PayloadDirectoryPath.SIPayloadDirectoryPath.ArchiveDestinationPath.ToString()), PayloadDirection.OUT));
        }

        public void CreateDirectoryInfo()
        {
            SourceInDirectoryInfo = FileServices.GetFileDirectory(SourcePath[Source.SI_INBOUND.ToString()]);
            SourceOutDirectoryInfo = FileServices.GetFileDirectory(SourcePath[Source.SI_OUTBOUND.ToString()]);
        }

        public string GetSourceFolder()
        {
            return DestinationPath[Source.SI_OUTBOUND.ToString()];
        }

        public string GetDestinationFolder()
        {
            return SourcePath[Source.SI_INBOUND.ToString()];
        }

        public string GetArchivedFolder()
        {
            return DestinationPath[Source.SI_INBOUND.ToString()];
        }

        public FileInfo[] GetInBoundPayloadFiles()
        {
            return InBoundPayloadFiles;
        }
    }
}
