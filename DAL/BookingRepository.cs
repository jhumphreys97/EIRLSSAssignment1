﻿using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EIRLSSAssignment1.DAL
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingContext _context;

        public BookingRepository(BookingContext context)
        {
            _context = context;
        }

        public IList<Booking> GetBookings()
        {
            return _context.Bookings.ToList();
        }

        public Booking GetBookingById(int id)
        {
            return _context.Bookings.Where(x => x.Id == id).SingleOrDefault();
        }

        public void Insert(Booking Booking)
        {
            _context.Bookings.Add(Booking);
        }

        public void Update(Booking Booking)
        {
            _context.Entry(Booking).State = EntityState.Modified;
        }

        public void Delete(Booking Booking)
        {
            _context.Bookings.Remove(Booking);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}