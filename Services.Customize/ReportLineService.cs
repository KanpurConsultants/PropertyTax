﻿using System.Collections.Generic;
using System.Linq;
using System;
using Model;
using System.Data.SqlClient;
using System.Configuration;
using Infrastructure.IO;
using Models.Customize.Models;
using Models.Customize.ViewModels;
using Models.BasicSetup.ViewModels;
using Models.BasicSetup.Models;
using Models.Customize.DataBaseViews;
using Models.Company.Models;
using Components.Logging;
using ProjLib.Constants;
using AutoMapper;
using Models.Company.ViewModels;
using Services.BasicSetup;
using ProjLib;
using System.Xml.Linq;
using Data.Customize;
using Model.Models;

namespace Services.Customize
{
    public interface IReportLineService : IDisposable
    {
        ReportLine Create(ReportLine pt);
        void Delete(int id);
        void Delete(ReportLine pt);
        ReportLine Find(int id);
        void Update(ReportLine pt);
        ReportLine Add(ReportLine pt);
        IEnumerable<ReportLine> GetReportLineList(int id);
        ReportLine GetReportLine(int id);
        ReportLine GetReportLineByName(string Name,int HeaderID);
    }

    public class ReportLineService : IReportLineService
    {
        private ApplicationDbContext db;
        private readonly IUnitOfWorkForService _unitOfWork;

        public ReportLineService(IUnitOfWorkForService unitOfWork)
        {
            this.db = new ApplicationDbContext();
            _unitOfWork = unitOfWork;
        }
        public ReportLine Find(int id)
        {
            return _unitOfWork.Repository<ReportLine>().Find(id);            
        }

        public ReportLine Create(ReportLine pt)
        {
            pt.ObjectState = ObjectState.Added;
            _unitOfWork.Repository<ReportLine>().Insert(pt);
            return pt;
        }
        public ReportLine GetReportLine(int id)
        {
            return ((from p in db.ReportLine
                        where p.ReportLineId==id
                        select p).FirstOrDefault()
                        );
        }
        public ReportLine GetReportLineByName(string Name,int HeaderID)
        {
            return (from p in db.ReportLine
                    where p.ReportHeaderId == HeaderID && p.FieldName == Name
                    select p
                        ).FirstOrDefault();
        }

        public void Delete(int id)
        {
            _unitOfWork.Repository<ReportLine>().Delete(id);
        }

        public void Delete(ReportLine pt)
        {
            _unitOfWork.Repository<ReportLine>().Delete(pt);
        }

        public void Update(ReportLine pt)
        {
            pt.ObjectState = ObjectState.Modified;
            _unitOfWork.Repository<ReportLine>().Update(pt);
        }

        public IEnumerable<ReportLine> GetReportLineList(int id)
        {
            var pt = _unitOfWork.Repository<ReportLine>().Query().Get().Where(m=>m.ReportHeaderId==id).OrderBy(m=>m.Serial);

            return pt;
        }

        public ReportLine Add(ReportLine pt)
        {
            _unitOfWork.Repository<ReportLine>().Insert(pt);
            return pt;
        }

        public void Dispose()
        {
        }

    }
}
