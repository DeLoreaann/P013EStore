﻿namespace P013EStore.WebAPIUsing.Utils
{
    public class FileHelper
    {
        public static async Task<string> FileLoaderAsync(IFormFile formFile, string filePath = "/Img/")
        {
            string fileName = "";
            fileName = formFile.FileName;
            string directory = Directory.GetCurrentDirectory() + "/wwwroot" + filePath + fileName;
            using var stream = new FileStream(directory, FileMode.Create);
            await formFile.CopyToAsync(stream);
            return fileName;
        }

		public static bool FileRemover(string fileName, string filePath = "/wwwroot/Img/")
		{
			string directory = Directory.GetCurrentDirectory() + "/wwwroot" + filePath + fileName;
			if (File.Exists(directory)) // File.Exist metodu .net içinde var olan ve kendisine verilen dizinde dosya var mı yok mu kotrol eden bir metottur.
			{
				File.Delete(directory); // File.Delete metodu bir dizinden dosya siler 
				return true; //  dosya silindikten sonra metot geriye true döner
			}
			return false; // yukarıdaki silme kodu çalışmazsa metot geriye false döner böylece işlem sonucundan haberdar olabiliriz.

		}


	}


    
}



