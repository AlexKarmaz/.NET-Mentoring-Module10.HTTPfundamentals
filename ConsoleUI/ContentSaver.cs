using SiteDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class ContentSaver : IContentSaver
    {
        private readonly DirectoryInfo rootDirectory;

        public ContentSaver(DirectoryInfo rootDirectory)
        {
            this.rootDirectory = rootDirectory;
        }

        public void SaveHtmlDocument(Uri uri, string name, Stream documentStream)
        {
            string directoryPath = CombineLocations(rootDirectory, uri);
            Directory.CreateDirectory(directoryPath);
            name = RemoveInvalidSymbols(name);
            string fileFullPath = Path.Combine(directoryPath, name);

            SaveToFile(documentStream, fileFullPath);
            documentStream.Close();
        }

        public void SaveFile(Uri uri, Stream fileStream)
        {
            string fileFullPath = CombineLocations(rootDirectory, uri);
            var directoryPath = Path.GetDirectoryName(fileFullPath);
            Directory.CreateDirectory(directoryPath);
            if (Directory.Exists(fileFullPath)) // if file name cannot be obtained from uri
            {
                fileFullPath = Path.Combine(fileFullPath, Guid.NewGuid().ToString());
            }

            SaveToFile(fileStream, fileFullPath);
            fileStream.Close();
        }

        private void SaveToFile(Stream stream, string fileFullPath)
        {
            var createdFileStream = File.Create(fileFullPath);
            stream.CopyTo(createdFileStream);
            createdFileStream.Close();
        }

        private string CombineLocations(DirectoryInfo directory, Uri uri)
        {
            return Path.Combine(directory.FullName, uri.Host) + uri.LocalPath.Replace("/", @"\");
        }

        private string RemoveInvalidSymbols(string filename)
        {
            var invalidSymbols = Path.GetInvalidFileNameChars();
            return new string(filename.Where(c => !invalidSymbols.Contains(c)).ToArray());
        }
    }
}
