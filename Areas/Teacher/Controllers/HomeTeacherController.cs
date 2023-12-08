using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using web2.Models;

namespace web2.Areas.Teacher.Controllers
{
    public class HomeTeacherController : Controller
    {
        private quanLyTrungTamDayDanEntities db = new quanLyTrungTamDayDanEntities();

        public ActionResult Manage_Document()
        {
            var documents = db.TaiLieux.ToList();
            return View(documents);
        }
        public ActionResult Manage_Classes()
        {
            // Assuming you have a DbContext called 'YourDbContext'
            using (var context = new quanLyTrungTamDayDanEntities())
            {
                // Fetch the data from the database
                var classes = context.LopHocs.ToList();

                // Pass the data to the view
                return View(classes);
            }
        }
        public ActionResult Manage_Schedule()
        {
            return View();
        }
        public ActionResult Index()
        {
            using (var context = new quanLyTrungTamDayDanEntities())
            {
                // Lấy danh sách tất cả giáo viên
                var allTeachers = context.GiaoViens.ToList();

                // Chọn một giáo viên ngẫu nhiên
                var randomTeacher = allTeachers[new Random().Next(allTeachers.Count)];

                // Truyền thông tin giáo viên đến view
                return View(randomTeacher);
            }
        }
        public ActionResult AddDocument()
        {
            return View(new TaiLieu());
        }

        public ActionResult ProcessAddDocument(TaiLieu newDocument)
        {
            try
            {
                // Validate ModelState
                if (!ModelState.IsValid)
                {
                    // Handle invalid ModelState (e.g., return to the form with validation errors)
                    return View("AddDocument", newDocument);
                }

                // Set default values
                newDocument.Ma_tai_lieu = Guid.NewGuid().ToString();
                newDocument.Ten_tai_lieu = Guid.NewGuid().ToString();
                newDocument.Ngay_upload = DateTime.UtcNow;
                newDocument.Duong_dan = Guid.NewGuid().ToString();
                newDocument.Ma_giang_vien = Guid.NewGuid().ToString();
                newDocument.Hoc_vien_duoc_xem = Guid.NewGuid().ToString();

                // Add the new document to the database
                db.TaiLieux.Add(newDocument);
                db.SaveChanges();

                return RedirectToAction("Manage_Document");
            }
            catch (DbEntityValidationException ex)
            {
                // Log validation errors
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var error in validationErrors.ValidationErrors)
                    {
                        Debug.WriteLine($"Property: {error.PropertyName} Error: {error.ErrorMessage}");
                    }
                }

                // Handle the exception (log, redirect, etc.)
                return RedirectToAction("Manage_Document");
            }
            catch (Exception ex)
            {
                // Log and handle other exceptions using Debug.WriteLine
                Debug.WriteLine($"An error occurred while processing the document. Error: {ex.Message}");
                return RedirectToAction("Manage_Document");
            }
        }
        public ActionResult EditDocument(string documentId)
        {
            try
            {
                var document = db.TaiLieux.Find(documentId);
                if (document != null)
                {
                    return View(document);
                }
                return RedirectToAction("Manage_Document");
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return RedirectToAction("Manage_Document");
            }
        }

        [HttpPost]
        public ActionResult ProcessEditDocument(TaiLieu editedDocument)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingDocument = db.TaiLieux.Find(editedDocument.Ma_tai_lieu);
                    if (existingDocument != null)
                    {
                        existingDocument.Ten_tai_lieu = editedDocument.Ten_tai_lieu;
                        existingDocument.Duong_dan = editedDocument.Duong_dan;
                        existingDocument.Ngay_upload = editedDocument.Ngay_upload;
                        existingDocument.Ma_giang_vien = editedDocument.Ma_giang_vien;
                        existingDocument.Hoc_vien_duoc_xem = editedDocument.Hoc_vien_duoc_xem;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Manage_Document");
                }

                return View("EditDocument", editedDocument);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return RedirectToAction("Manage_Document");
            }
        }

        public ActionResult DeleteDocument(string documentId)
        {
            try
            {
                var document = db.TaiLieux.Find(documentId);
                if (document != null)
                {
                    return View(document);
                }

                return RedirectToAction("Manage_Document");
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return RedirectToAction("Manage_Document");
            }
        }

        [HttpPost]
        public ActionResult ProcessDeleteDocument(TaiLieu documentId)
        {

            try
            {
                var document = db.TaiLieux.Find(documentId.Ma_tai_lieu);
                if (document != null)
                {
                    db.TaiLieux.Remove(document);
                    db.SaveChanges();
                    db.Entry(document).State = EntityState.Detached;
                    var documents = db.TaiLieux.ToList();
                    return View("Manage_Document", documents);
                }

                return RedirectToAction("Manage_Document");
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return RedirectToAction("Manage_Document");
            }
        }
    }
}
