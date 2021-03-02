﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using Microsoft.AspNet.Identity;
using EIRLSSAssignment1.Customisations;
using System.IO;

namespace EIRLSSAssignment1.ServiceLayer
{
    public class DrivingLicenseService
    {
        private readonly DrivingLicenseRepository _drivingLicenseRepository;
        private readonly ApplicationDbContext _applicationDbContext;

        public DrivingLicenseService()
        {
            _drivingLicenseRepository = new DrivingLicenseRepository(new ApplicationDbContext());
            _applicationDbContext = new ApplicationDbContext();
        }

        public IList<DrivingLicense> GetIndex()
        {
            return _drivingLicenseRepository.GetDrivingLicenses().ToList();
        }

        public DrivingLicense GetDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected an integer");
            }
            DrivingLicense drivingLicense = _drivingLicenseRepository.GetDrivingLicenseById(id);
            if (drivingLicense == null)
            {
                throw new DrivingLicenseNotFoundException("Driving License not found.");
            }

            return drivingLicense;
        }

        public DrivingLicenseViewModel CreateView()
        {
            DrivingLicenseViewModel drivingLicenseVM = new DrivingLicenseViewModel();

            if(HttpContext.Current.User.IsInRole("Admin"))
            {
                drivingLicenseVM.Users = new SelectList(_applicationDbContext.Users.ToList(), "Id", "Name");
                return drivingLicenseVM;
            }
            else
            {
                drivingLicenseVM.userId = HttpContext.Current.User.Identity.GetUserId();
                return drivingLicenseVM;
            }
        }

        public ServiceResponse CreationAction(DrivingLicenseViewModel drivingLicenseVM)
        {

            if (drivingLicenseVM == null)
            {
                return new ServiceResponse { Result = false, ServiceObject = null };

            }

            _drivingLicenseRepository.Insert(drivingLicenseVM.License);
            _drivingLicenseRepository.Save();

            return new ServiceResponse { Result = true, ServiceObject = null };
        }

        public DrivingLicenseViewModel EditView(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            DrivingLicense drivingLicense = _drivingLicenseRepository.GetDrivingLicenseById(id);
            if (drivingLicense == null)
            {
                throw new DrivingLicenseNotFoundException("Driving License not found");
            }

            var drivingLicenseVM = new DrivingLicenseViewModel { License = drivingLicense, ImageToUpload = null };

            return drivingLicenseVM;
        }

        public ServiceResponse EditAction(DrivingLicenseViewModel drivingLicenseVM)
        {
            if (drivingLicenseVM == null)
            {
                return new ServiceResponse { Result = false, ServiceObject = null };
            }

            _drivingLicenseRepository.Update(drivingLicenseVM.License);
            _drivingLicenseRepository.Save();

            return new ServiceResponse { Result = true, ServiceObject = null };
        }

        public DrivingLicense DeleteView(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            DrivingLicense drivingLicense = _drivingLicenseRepository.GetDrivingLicenseById(id);
            if (drivingLicense == null)
            {
                throw new DrivingLicenseNotFoundException("Driving License not found.");
            }
            return drivingLicense;
        }

        public ServiceResponse DeleteAction(int id)
        {
            DrivingLicense drivingLicense = _drivingLicenseRepository.GetDrivingLicenseById(id);

            _drivingLicenseRepository.Delete(drivingLicense);
            _drivingLicenseRepository.Save();

            return new ServiceResponse { Result = true, ServiceObject = null };
        }

        public void Dispose()
        {
            _drivingLicenseRepository.Save();
        }

        private byte[] convertImageToByteArray(HttpPostedFileBase image)
        {
            byte[] imageByteArray = null;

            if (image != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.InputStream.CopyTo(memoryStream);
                    imageByteArray = memoryStream.GetBuffer();
                }
            }

            return imageByteArray;
        }
    }
}