using SimulationThreeWebApp.Utilities.Enums;


namespace SimulationThreeWebApp.Utilities.Extensions;

public static class Validator
{
    public static bool ValidateType(this IFormFile file, string type)
    {
        if (file.ContentType.Contains(type))
        {
            return true;
        }
        return false;
    }

    public static bool ValidateSize(this IFormFile file, FileSize fileSize, int size)
    {
        switch (fileSize)
        {
            case FileSize.KB:
                return file.Length <= size * 1024;
            case FileSize.MB:
                return file.Length <= size * 1024 * 1024;
            case FileSize.GB:
                return file.Length <= size * 1024 * 1024 * 1024;
        }
        return false;
    }

    public static async Task<string> CreateFileAsync(this IFormFile formFile, params string[] roots)
    {
        string fileName = String.Concat(Guid.NewGuid().ToString(), Path.GetExtension(formFile.FileName));
        string path = string.Empty;

        for (int i = 0; i < roots.Length; i++)
        {
            path = Path.Combine(path, roots[i]);
        }

        path = Path.Combine(path, fileName);

        using (FileStream fileStream = new(path, FileMode.Create))
        {
            await formFile.CopyToAsync(fileStream);
        }

        return fileName;
    }

    public static void DeleteFile(this string filename, params string[] roots)
    {
        string path = string.Empty;
        for (int i = 0; i < roots.Length; i++)
        {
            path = Path.Combine(path, roots[i]);
        }
        path = Path.Combine(path, filename);

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}