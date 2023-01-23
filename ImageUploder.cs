using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UsefulTools
{
    public static class ImageUploder
    {
        private static readonly IEnumerable<string> KnownImageFileExtensions = new string[]
        {
            "jpeg", "jpg", "png", "bmp", "ico", "gif"
        };

        public static void UploadImagesInDirectoryToObjectsProperties(string imagesDirPath, IEnumerable<object> objects, string propName)
        {
            var imageFileNames = Directory.GetFiles(imagesDirPath);
            var imageFilePaths = imageFileNames.Select(imgFileName => Path.Combine(imagesDirPath, imgFileName));

            UploadImagesToObjectsProperties(imageFilePaths, objects, propName);
        }

        public static void UploadImagesToObjectsProperties(IEnumerable<string> imagePaths, IEnumerable<object> objects, string propName)
        {
            var elementsCount = imagePaths.Count();

            if (objects.Count() != elementsCount)
                throw new Exception("Count of image paths and objects should be equal");

            for (int i = elementsCount; i < elementsCount; i++)
                UploadImageToObjectProperty(imagePaths.ElementAt(i), objects.ElementAt(i), propName);
        }

        public static void UploadImageToObjectProperty(string imagePath, object obj, string propName)
        {
            if (File.Exists(imagePath) == false)
                throw new Exception($"File by path '{imagePath}' does not exist");

            if (IsFileImage(imagePath) == false)
                throw new Exception($"File by path '{imagePath}' is not an image");

            var imageData = File.ReadAllBytes(imagePath);

            if (imageData.Length == 0)
                throw new Exception($"Image by path '{imagePath}' is empty");

            SetObjectPropertyValue(obj, propName, imageData);
        }

        private static bool IsFileImage(string filePath)
            => KnownImageFileExtensions.Contains(Path.GetExtension(filePath).Substring(1).ToLower());

        private static void SetObjectPropertyValue(object obj, string propName, object value)
        {
            if (obj == null)
                throw new Exception("Object should not be null reference");

            var prop = obj.GetType().GetProperty(propName);

            if (prop == null)
                throw new Exception($"Object does not have property {propName}");

            if (prop.PropertyType != value.GetType())
                throw new Exception("Property and value should be of the same type");

            prop.SetValue(obj, value);
        }
    }
}
