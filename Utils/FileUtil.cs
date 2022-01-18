using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Utils.CommonEnums;

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
        public static async Task<string> GenerateFileAsync(string folderPath, IFormFile formFile, FileType fileType)
        {
            var fileName = $"{Guid.NewGuid()}-{formFile.FileName}";
            var filePath = Path.Combine(folderPath, fileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            FileStream stream = new FileStream(filePath, FileMode.Open);
            await FireBaseUploadAsync(stream, fileName, fileType);
            stream.Close();

            return fileName;
        }

        public static async Task DeleteFileAsync(string fileName, FileType fileType)
        {
            var path = Path.Combine(fileType == FileType.Image ? Constants.ImageFolderPath : Constants.VideoFolderPath, fileName);
            if (File.Exists(path))
            {
                await FireBaseDeleteAsync(fileName, fileType);
                File.Delete(path);
            }
        }

        public static async Task<string> UpdateFileAsync(string fileName, string folderPath, IFormFile formFile, FileType fileType)
        {
            await DeleteFileAsync(fileName, fileType);
            var newFileName = await GenerateFileAsync(folderPath, formFile, fileType);

            return newFileName;
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
            , string buttonName, string homeLink)
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

        public static async Task FireBaseUploadAsync(FileStream stream, string fileName, FileType fileType)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(Constants.ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(Constants.AuthEmail, Constants.AuthPassword);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                Constants.Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child(fileType == FileType.Image ? "images" : "videos")
                .Child(fileName)
                .PutAsync(stream, cancellation.Token);

            try
            {
                string link = await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}", ex);
            }
        }

        public static async Task FireBaseDeleteAsync(string fileName, FileType fileType)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(Constants.ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(Constants.AuthEmail, Constants.AuthPassword);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                Constants.Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child(fileType == FileType.Image ? "images" : "videos")
                .Child(fileName)
                .DeleteAsync();
        }

        public static async Task<string> FireBaseGetAsync(string fileName, FileType fileType)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(Constants.ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(Constants.AuthEmail, Constants.AuthPassword);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                Constants.Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child(fileType == FileType.Image ? "images" : "videos")
                .Child(fileName)
                .GetDownloadUrlAsync();

            return await task;
        }
    }
}
