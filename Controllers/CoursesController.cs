using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElearningMVC.Data;
using ElearningMVC.Models;
using Amazon.S3;
using Amazon;
using Amazon.S3.Model;
using ElearningMVC.ViewModel;
using System.Drawing;

namespace ElearningMVC.Controllers
{
    public class CoursesController : Controller
    {
        private static IAmazonS3 client;
        private const string bucketName = "elearnings3storage";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast1;

        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.ToListAsync();
            var coursesViewModelList = new List<CourseViewModel>();
            foreach (var course in courses)
            {
                MemoryStream rs = new MemoryStream();
                client = new AmazonS3Client(awsAccessKeyId: "AKIAR4FED76S6ZSRLMGJ", "nyWqKNMpFWDonbE2lGNjYtrefz5XtQrf9J4TNq8E", bucketRegion);
                GetObjectRequest getObjectRequest = new GetObjectRequest();
                getObjectRequest.BucketName = bucketName;
                getObjectRequest.Key = course.PhotoURL;
                string key = "";
                using (var getObjectResponse = await client.GetObjectAsync(getObjectRequest))
                {
                    getObjectResponse.ResponseStream.CopyTo(rs);
                    key = getObjectResponse.Key;
                }


                coursesViewModelList.Add(new CourseViewModel()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Code = course.Code,
                    Key = key
                });
            }


            return View(coursesViewModelList);
        }

        public static byte[] ReadStream(Stream responseStream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Code,TeacherJoinClassId")] Course course, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                client = new AmazonS3Client(awsAccessKeyId: "AKIAR4FED76S6ZSRLMGJ", "nyWqKNMpFWDonbE2lGNjYtrefz5XtQrf9J4TNq8E", bucketRegion);
                WritingAnObjectAsync(file).Wait();

                course.PhotoURL = file.FileName;
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [HttpPost]
        public async Task<ActionResult> AddCoursePhoto(IFormFile file)
        {
            client = new AmazonS3Client("AKIAR4FED76S6ZSRLMGJ", "nyWqKNMpFWDonbE2lGNjYtrefz5XtQrf9J4TNq8E", bucketRegion);
            WritingAnObjectAsync(file).Wait();
            return RedirectToAction("UploadSuccess");
        }

        static async Task WritingAnObjectAsync(IFormFile file)
        {
            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = file.FileName,
                    ContentBody = "sample text"
                };

                PutObjectResponse response1 = await client.PutObjectAsync(putRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code,PhotoURL,TeacherJoinClassId")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
