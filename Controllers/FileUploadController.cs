using Microsoft.AspNetCore.Mvc;

namespace FileUpload_MVC.Controllers;

public class FileUploadController : Controller
{
    private readonly IWebHostEnvironment _environment;

    public FileUploadController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    // POSt
    [HttpPost("FileUpload")]
    public async Task<IActionResult> Index(List<IFormFile> files)
    {
        var contentPath = _environment.ContentRootPath;
        var filePath = Path.Combine(_environment.ContentRootPath, "Uploads");

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        foreach (IFormFile formFile in files)
        {
            if (formFile.Length > 0)
            {
                // full path to file in temp location
                var fileName = Path.GetFileName(formFile.FileName);
                using (var stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                    ViewBag.Message += fileName + ",";
                }
            }
        }

        // process uploaded files
        // Don't rely on or trust the FileName property without validation.
        return View();
    }
}