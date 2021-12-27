#nullable enable

using System;
using System.IO;

namespace UM.SourceGeneration
{
    public class FileWriter
    {
        public virtual string Path => "Assets/Scripts/Generated/";

        public virtual string GetFilePath(Type type, string suffix = "")
        {
            if (string.IsNullOrEmpty(type.FullName))
            {
                throw new ArgumentException("Provided type's FullName is null.");
            }

            return Path + type.FullName!.Replace('.', '/') + suffix + ".cs";
        }

        public virtual void WriteAllText(string path, string contents)
        {
            string directoryPath = path.Substring(0, path.LastIndexOf('/'));

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(path, contents);
        }
    }
}
