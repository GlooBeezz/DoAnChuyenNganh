using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
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
           
            using (var context = new quanLyTrungTamDayDanEntities())
            {
               
                var classes = context.LopHocs.ToList();

                return View(classes);
            }
        }
        public ActionResult Manage_Schedule()
        {
            List<LopHoc> ds = new List<LopHoc>();
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["quanLyTrungTamDayDanEntities2"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            try
            {
                SqlCommand Cmd = new SqlCommand("select Ten_lop_hoc,Hoc_vao_thu, Thoi_gian_bat_dau,Thoi_gian_ket_thuc,Phong_hoc,Ma_lop from LopHoc join ThoiGianBieu on LopHoc.Ma_thoi_gian_bieu=ThoiGianBieu.Ma_thoi_gian_bieu", conn);
                SqlDataReader dr = Cmd.ExecuteReader();
                while (dr.Read())
                {
                    LopHoc lop = new LopHoc
                    {
                        Ma_lop = dr["Ma_lop"].ToString(),
                        Ten_lop_hoc = dr["Ten_lop_hoc"].ToString(),
                        Hoc_vao_thu = dr["Hoc_vao_thu"].ToString(),
                        Thoi_gian_bat_dau = (TimeSpan)dr["Thoi_gian_bat_dau"],
                        Thoi_gian_ket_thuc = (TimeSpan)dr["Thoi_gian_ket_thuc"],
                        Phong_hoc = dr["Phong_hoc"].ToString()
                    };
                    ds.Add(lop);
                }
                return View( ds);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                return View("Index");
            }
        }
        public ActionResult Index()
        {
            selectedTeacher = GetRandomTeacher();
            return View(selectedTeacher);

        }
        public ActionResult AddDocument()
        {
            return View(new TaiLieu());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessAddDocument(TaiLieu newDocument)
        {
            if (!ModelState.IsValid)
            {
                return View("AddDocument", newDocument);
            }
            try
            {
                db.TaiLieux.Add(newDocument);
                db.SaveChanges();

                return RedirectToAction("Manage_Document");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var error in validationErrors.ValidationErrors)
                    {
                        Debug.WriteLine($"Property: {error.PropertyName} Error: {error.ErrorMessage}");
                    }
                }
                return RedirectToAction("Manage_Document");
            }
            catch (Exception ex)
            {
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
                
                return RedirectToAction("Manage_Document");
            }
        }
        private GiaoVien selectedTeacher;
        public ActionResult MyClass(string teacherId)
        {
            var teacher = string.IsNullOrEmpty(teacherId) ? GetRandomTeacher() : db.GiaoViens.Find(teacherId);

            if (teacher == null)
            {
                return RedirectToAction("Index");
            }
            var classesTaughtByTeacher = db.LopHocs
                .Include(c => c.HocViens)
                .Where(c => c.Ma_lop == teacher.Ma_lop_giang_day)
                .ToList();
            return View(classesTaughtByTeacher);
        }




        private GiaoVien GetRandomTeacher()
        {
            using (var context = new quanLyTrungTamDayDanEntities())
            {
                var allTeachers = context.GiaoViens.ToList();
                var randomTeacher = allTeachers[new Random().Next(allTeachers.Count)];
                return randomTeacher;
            }
        }
    }
}
