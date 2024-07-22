﻿namespace FiorelloApp.Areas.AdminArea.Helpers
{
    public class Helper
    {
        public static void DeleteImageFromFolder(string fileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}
