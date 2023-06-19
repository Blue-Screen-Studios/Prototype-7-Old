using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assembly.IBX.WebIO
{
    public static class IOSystem
    {
        public enum ImageFileFormat { EXR, JPG, PNG, TGA };
        private const long MEGABYTE_SIZE = 1000000;

        /// <summary>
        /// Get a file extension for an image file format
        /// </summary>
        /// <param name="format">The image file format</param>
        /// <returns>An extension for the image file format</returns>
        public static string GetImageFileExtension(ImageFileFormat format)
        {
            switch(format)
            {
                case ImageFileFormat.EXR:
                    return "exr";
                case ImageFileFormat.JPG:
                    return "jpg";
                case ImageFileFormat.PNG:
                    return "png";
                case ImageFileFormat.TGA:
                    return "tga";
                default:
                    return "ibx";
            }
        }

        private static string GetFullPath(string path)
        {
            char firstChar = path[0];

            if(firstChar == '/')
            {
                return Application.streamingAssetsPath + path;
            }
            else
            {
                return Application.streamingAssetsPath + '/' + path;
            }
        }

        private static bool IsRoomOnDrive(string path, byte[] data)
        {
            //Get the path of our main exe file
            string instalationPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            //Get the path root (drive letter) for the installation path
            string drivePath = Path.GetPathRoot(instalationPath);

            //Calculate the space availible
            DriveInfo installationDrive = new DriveInfo(drivePath);
            
            //Calculate the space availible
            long spaceAvailible = installationDrive.AvailableFreeSpace;
            long spaceRequired = data.Length;

            //Return wether or not there is room on the drive even with 1MB more writes than anticipated
            return spaceRequired + MEGABYTE_SIZE < spaceAvailible;
        }

        private static bool IsRoomOnDrive(string path, string data, Encoding encoding)
        {
            //Get the path of our main exe file
            string instalationPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            
            //Get the path root (drive letter) for the installation path
            string drivePath = Path.GetPathRoot(instalationPath);

            //Calculate the space availible
            DriveInfo installationDrive = new DriveInfo(drivePath);

            //Calculate space required and availible
            long spaceAvailible = installationDrive.AvailableFreeSpace;
            long spaceRequired = encoding.GetBytes(data).Length;

            //Return wether or not there is room on the drive even with 1MB more writes than anticipated
            return spaceRequired + MEGABYTE_SIZE < spaceAvailible;
        }

        /// <summary>
        /// Safely writes a binary file to disk
        /// </summary>
        /// <param name="partialPath">The partial path to write to</param>
        /// <param name="data">The data that will go inside of the file</param>
        public static void SafeWriteBytes(string partialPath, byte[] data)
        {
            string writePath = GetFullPath(partialPath);

            bool enoughSpace = IsRoomOnDrive(writePath, data);

            if(enoughSpace)
            {
                File.WriteAllBytes(writePath, data);
            }
        }

        /// <summary>
        /// Safely writes a text file to disk
        /// </summary>
        /// <param name="partialPath">The partial path to write to</param>
        /// <param name="content">The text to write inside the file</param>
        /// <param name="encoding">The method of encoding used for storing text</param>
        public static void SafeWriteContent(string partialPath, string content, Encoding encoding)
        {
            string writePath = GetFullPath(partialPath);

            bool enoughSpace = IsRoomOnDrive(writePath, content, encoding);

            if(enoughSpace)
            {
                File.WriteAllText(writePath, content, encoding);
            }
        }

        /// <summary>
        /// Write a texture asset to disk as an exr, jpg, png, or tga file
        /// </summary>
        /// <param name="texture">The texture to write to disk</param>
        /// <param name="partialPath">The parital path to write the texture to</param>
        /// <param name="format">The image file format to write in</param>
        /// <param name="quality">Optional - The compression quality of the image (applicable for JPG)</param>
        public static void SafeWriteTexture(Texture2D texture, string partialPath, ImageFileFormat format, int quality = 95)
        {
            UnityEngine.Debug.Log($"{partialPath} writing texture to disk");
            
            switch(format)
            {
                case ImageFileFormat.EXR:
                    SafeWriteBytes($"{partialPath}.exr", texture.EncodeToEXR());
                    break;
                case ImageFileFormat.JPG:
                    SafeWriteBytes($"${partialPath}.jpg", texture.EncodeToJPG(quality));
                    break;
                case ImageFileFormat.PNG:
                    SafeWriteBytes($"{partialPath}.png", texture.EncodeToPNG());
                    break;
                default:
                    SafeWriteBytes($"{partialPath}.tga", texture.EncodeToTGA());
                    break;
            }
        }
    }
}
