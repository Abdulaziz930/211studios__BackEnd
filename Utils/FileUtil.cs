using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class FileUtil
    {
        /// <summary>
        /// Creates a file and adds a path
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="formFile"></param>
        /// <returns>string</returns>
        public static async Task<string> GenerateFileAsync(string folderPath, IFormFile formFile)
        {
            var fileName = $"{Guid.NewGuid()}-{formFile.FileName}";
            var filePath = Path.Combine(folderPath, fileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return fileName;
        }

        /// <summary>
        /// Creates a file and adds pathes
        /// </summary>
        /// <param name="folderPaths"></param>
        /// <param name="formFile"></param>
        /// <returns>string</returns>
        public static async Task<string> GenerateFileAsync(List<string> folderPaths, IFormFile formFile)
        {
            var fileName = $"{Guid.NewGuid()}-{formFile.FileName}";
            var filePath = "";
            foreach (var folderPath in folderPaths)
            {
                filePath = Path.Combine(folderPath, fileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
            }

            return fileName;
        }

        /// <summary>
        /// Reades html file and create email html view
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="link"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="buttonName"></param>
        /// <param name="homeLink"></param>
        /// <returns>string</returns>
        public static string GetEmailView(string filePath, string link, string title, string description
            , string buttonName,string homeLink)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string mailText = streamReader.ReadToEnd();
            streamReader.Close();

            mailText = mailText.Replace("{title}", title).Replace("{description}", description)
                .Replace("{button}", buttonName).Replace("{link}", link).Replace("{homeLink}", homeLink);

            return mailText;
        }

        public static string GetContactEmailView(string filePath, string title, string description, string homeLink)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string mailText = streamReader.ReadToEnd();
            streamReader.Close();

            mailText = mailText.Replace("{title}", title).Replace("{description}", description).Replace("{homeLink}", homeLink);

            return mailText;
        }
    }
}
