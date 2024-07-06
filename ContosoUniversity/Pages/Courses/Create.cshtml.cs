using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ContosoUniversity.Pages.Courses
{
    public class CreateModel : DepartmentNamePageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public CreateModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopulateDepartmentsDropDownList(_context);
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var emptyCourse = new Course();

                if (await TryUpdateModelAsync<Course>(
                     emptyCourse,
                     "course",   // Prefix for form value.
                     s => s.CourseID, s => s.DepartmentID, s => s.Title, s => s.Credits))
                {
                    _context.Courses.Add(emptyCourse);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                else
                {
                    var validationErrors = ModelState.Values.Where(E => E.Errors.Count > 0)
                        .SelectMany(E => E.Errors)
                        .Select(E => E.ErrorMessage)
                        .ToList();

                }

                // Select DepartmentID if TryUpdateModelAsync fails.
                PopulateDepartmentsDropDownList(_context, emptyCourse.DepartmentID);
                //return Page();
            }
            catch (Exception ex)
            {
            }
            return Page();
        }
    }
}