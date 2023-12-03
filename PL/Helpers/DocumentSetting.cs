namespace PL.Helpers
{
    public class DocumentSetting
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            //1- get located folder path
            string folderpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);
            //2- get filename and make it unique
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            //3- get filepath
            string filePath = Path.Combine(folderpath, fileName);
            //4-save file as stream
            using FileStream fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            return fileName;
        }
        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\files",folderName, fileName);
            if(File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
