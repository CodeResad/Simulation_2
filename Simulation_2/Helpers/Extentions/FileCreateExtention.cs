namespace Simulation_2.Helpers.Extentions
{
    public static class FileCreateExtentions
    {
        public static string CreateFile(this IFormFile file, string webRoot, string FolderName)
        {
            string filename = "";

            if (file.FileName.Length > 64)
            {
                filename = Guid.NewGuid() + file.FileName.Substring(file.FileName.Length - 64);
            }
            else
            {
                filename = Guid.NewGuid() + file.FileName;
            }

            string path = Path.Combine(webRoot, FolderName, filename);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return filename;
        }

        public static void RemoveFile(this string FileName, string webRoot, string FolderName)
        {
            string path = Path.Combine(webRoot, FolderName, FileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
