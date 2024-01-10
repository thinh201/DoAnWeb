namespace DoAnWeb.Utilities
{
    public static class UploadImage
    {
        // Upload multiple images into default folder "/uploads/images"
        public static string? UploadSingleImage(IFormFile? file)
        {
            // Check if file is null or empty
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images", fileName);
            // Copy file to path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Example: /uploads/images/fileName
            // Return path to save in database
            return filePath.Substring(filePath.IndexOf("/uploads", StringComparison.Ordinal));
        }

        // Upload single image with custom folder name
        public static string? UploadSingleImage(IFormFile? file, string? folderName)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            string storeFolder = folderName ?? "images";
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            // Check if folder exists in wwwroot/uploads/storeFolder
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", storeFolder);
            // If folder doesn't exist, create it in wwwroot/uploads/storeFolder
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", storeFolder, fileName);
            // Copy file to path
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Example: /uploads/storeFolder/fileName
            // Return path to save in database
            return path.Substring(path.IndexOf("/uploads", StringComparison.Ordinal));
        }

        // Upload multiple images with custom folder name
        public static List<string>? UploadMultipleImages(List<IFormFile>? files, string? folderName)
        {
            if (files == null || files.Count == 0)
            {
                return null;
            }

            string storeFolder = folderName ?? "images";
            var listPath = new List<string>();
            foreach (var file in files)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                // Check if folder exists in wwwroot/uploads/storeFolder
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", storeFolder);
                // If folder doesn't exist, create it in wwwroot/uploads/storeFolder
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", storeFolder, fileName);
                // Copy file to path
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Example: /uploads/storeFolder/fileName
                // Return path to save in database
                listPath.Add(path.Substring(path.IndexOf("/uploads", StringComparison.Ordinal)));
            }

            return listPath;
        }

        // Upload multiple images with default folder name
        public static List<string>? UploadMultipleImages(List<IFormFile>? files)
        {
            if (files == null || files.Count == 0)
            {
                return null;
            }

            var listPath = new List<string>();
            foreach (var file in files)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                // Check if folder exists in wwwroot/uploads/storeFolder
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images");
                // If folder doesn't exist, create it in wwwroot/uploads/storeFolder
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images", fileName);
                // Copy file to path
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Example: /uploads/images/fileName
                // Return path to save in database
                listPath.Add(path.Substring(path.IndexOf("/uploads", StringComparison.Ordinal)));
            }

            return listPath;
        }
    }
}