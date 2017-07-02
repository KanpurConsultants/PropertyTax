using Infrastructure.IO;
using Models.BasicSetup.Models;
using Models.BasicSetup.ViewModels;
using Service;
using Services.BasicSetup;
using System;
using System.Collections.Generic;

namespace Services.Customize
{
    public interface ICustomizeComboHelpListService : IDisposable
    {
        ComboBoxResult GetPersonById(int Id);

        ComboBoxPagedResult GetEmployees(string searchTerm, int pageSize, int pageNum);
        ComboBoxPagedResult GetEmployeesWithProcess(string searchTerm, int pageSize, int pageNum, int ProcessId);
        ComboBoxResult GetEmployeeById(int Id);
        List<ComboBoxResult> GetMultipleEmployees(string Id);
        ComboBoxPagedResult GetGodowns(string searchTerm, int pageSize, int pageNum);
        ComboBoxResult GetGodownById(int Id);
        List<ComboBoxResult> GetMultipleGodowns(string Id);
        ComboBoxPagedResult GetMachines(string searchTerm, int pageSize, int pageNum);
        ComboBoxResult GetMachineById(int Id);
        List<ComboBoxResult> GetMultipleMachines(string Id);
        ComboBoxPagedResult GetDimension1(string searchTerm, int pageSize, int pageNum);
        ComboBoxResult GetDimension1ById(int Id);
        List<ComboBoxResult> GetMultipleDimension1(string Id);
        ComboBoxPagedResult GetDimension2(string searchTerm, int pageSize, int pageNum);
        ComboBoxResult GetDimension2ById(int Id);
        List<ComboBoxResult> GetMultipleDimension2(string Id);
        ComboBoxPagedResult GetProcess(string searchTerm, int pageSize, int pageNum);
        ComboBoxResult GetProcessById(int Id);
        List<ComboBoxResult> GetMultipleProcess(string Id);
        ComboBoxPagedResult GetProduct(string searchTerm, int pageSize, int pageNum);
        ComboBoxResult GetProductById(int Id);
        List<ComboBoxResult> GetMultipleProduct(string Id);
        ComboBoxPagedResult GetPersonWithProcess(string searchTerm, int pageSize, int pageNum, int ProcessId);
        ComboBoxPagedResult GetPersonWithDocType(string searchTerm, int pageSize, int pageNum, int DocTypeId);
        ComboBoxPagedResult GetPerson(string searchTerm, int pageSize, int pageNum);
        ComboBoxPagedResult GetBinLocation(string searchTerm, int pageSize, int pageNum);
        ComboBoxPagedResult GetBinLocation(string searchTerm, int pageSize, int pageNum, int filterid);
        ComboBoxResult GetBinLocationById(int Id);
        ComboBoxPagedResult GetArea(string searchTerm, int pageSize, int pageNum);
        ComboBoxResult GetAreaById(int Id);
        ComboBoxPagedResult GetPersonRateGroup(string searchTerm, int pageSize, int pageNum);
        ComboBoxResult GetPersonRateGroupById(int Id);


        ComboBoxPagedResult GetBillingType(string searchTerm, int pageSize, int pageNum);
        ComboBoxResult GetBillingTypeById(string Id);


        ComboBoxPagedResult GetLedgerAccount(string searchTerm, int pageSize, int pageNum);
        ComboBoxResult GetLedgerAccountById(int Id);


        ComboBoxPagedResult GetPaymentMode(string searchTerm, int pageSize, int pageNum);
        ComboBoxResult GetPaymentModeById(int Id);


        ComboBoxPagedResult GetCostCenter(string searchTerm, int pageSize, int pageNum);
        ComboBoxResult GetCostCenterById(int Id);
    }

    public class CustomizeComboHelpListService : ICustomizeComboHelpListService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomizeComboHelpListService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public ComboBoxPagedResult GetEmployees(string searchTerm, int pageSize, int pageNum)
        {
            return new ComboHelpListService(_unitOfWork).GetEmployees(searchTerm, pageSize, pageNum);
        }

        public ComboBoxPagedResult GetEmployeesWithProcess(string searchTerm, int pageSize, int pageNum, int ProcessId)
        {
            return new ComboHelpListService(_unitOfWork).GetEmployeesWithProcess(searchTerm, pageSize, pageNum, ProcessId);
        }

        public ComboBoxResult GetEmployeeById(int Id)
        {
            return new ComboHelpListService(_unitOfWork).GetEmployee(Id);
        }

        public List<ComboBoxResult> GetMultipleEmployees(string Id)
        {
            return new ComboHelpListService(_unitOfWork).GetMultipleEmployees(Id);
        }

        public ComboBoxPagedResult GetGodowns(string searchTerm, int pageSize, int pageNum)
        {
            return new GodownService(_unitOfWork).GetList(searchTerm, pageSize, pageNum);
        }

        public ComboBoxResult GetGodownById(int Id)
        {
            return new GodownService(_unitOfWork).GetValue(Id);
        }

        public List<ComboBoxResult> GetMultipleGodowns(string Id)
        {
            return new GodownService(_unitOfWork).GetListCsv(Id);
        }

        public ComboBoxPagedResult GetMachines(string searchTerm, int pageSize, int pageNum)
        {
            return new ProductService(_unitOfWork).GetMachineList(searchTerm, pageSize, pageNum);
        }

        public ComboBoxResult GetMachineById(int Id)
        {
            return new ProductService(_unitOfWork).GetValue(Id);
        }

        public List<ComboBoxResult> GetMultipleMachines(string Id)
        {
            return new ProductService(_unitOfWork).GetListCsv(Id);
        }

        public ComboBoxPagedResult GetDimension1(string searchTerm, int pageSize, int pageNum)
        {
            return new Dimension1Service(_unitOfWork).GetList(searchTerm, pageSize, pageNum);
        }

        public ComboBoxResult GetDimension1ById(int Id)
        {
            return new Dimension1Service(_unitOfWork).GetValue(Id);
        }

        public List<ComboBoxResult> GetMultipleDimension1(string Id)
        {
            return new Dimension1Service(_unitOfWork).GetListCsv(Id);
        }

        public ComboBoxPagedResult GetDimension2(string searchTerm, int pageSize, int pageNum)
        {
            return new Dimension2Service(_unitOfWork).GetList(searchTerm, pageSize, pageNum);
        }

        public ComboBoxResult GetDimension2ById(int Id)
        {
            return new Dimension2Service(_unitOfWork).GetValue(Id);
        }

        public List<ComboBoxResult> GetMultipleDimension2(string Id)
        {
            return new Dimension2Service(_unitOfWork).GetListCsv(Id);
        }

        public ComboBoxPagedResult GetProcess(string searchTerm, int pageSize, int pageNum)
        {
            return new ProcessService(_unitOfWork).GetList(searchTerm, pageSize, pageNum);
        }

        public ComboBoxResult GetProcessById(int Id)
        {
            return new ProcessService(_unitOfWork).GetValue(Id);
        }

        public List<ComboBoxResult> GetMultipleProcess(string Id)
        {
            return new ProcessService(_unitOfWork).GetListCsv(Id);
        }

        public ComboBoxPagedResult GetProduct(string searchTerm, int pageSize, int pageNum)
        {
            return new ProductService(_unitOfWork).GetList(searchTerm, pageSize, pageNum);
        }

        public ComboBoxResult GetProductById(int Id)
        {
            return new ProductService(_unitOfWork).GetValue(Id);
        }

        public List<ComboBoxResult> GetMultipleProduct(string Id)
        {
            return new ProductService(_unitOfWork).GetListCsv(Id);
        }
        public ComboBoxPagedResult GetPersonWithProcess(string searchTerm, int pageSize, int pageNum, int ProcessId)
        {
            return new PersonService(_unitOfWork).GetListWithProcess(searchTerm, pageSize, pageNum, ProcessId);
        }

        public ComboBoxPagedResult GetPersonWithDocType(string searchTerm, int pageSize, int pageNum, int DocTypeId)
        {
            return new PersonService(_unitOfWork).GetListWithDocType(searchTerm, pageSize, pageNum, DocTypeId);
        }

        public ComboBoxPagedResult GetPerson(string searchTerm, int pageSize, int pageNum)
        {
            return new PersonService(_unitOfWork).GetList(searchTerm, pageSize, pageNum);
        }


        public ComboBoxResult GetPersonById(int Id)
        {
            return new PersonService(_unitOfWork).GetValue(Id);
        }
        public ComboBoxPagedResult GetBinLocation(string searchTerm, int pageSize, int pageNum)
        {
            return new BinLocationService(_unitOfWork).GetList(searchTerm, pageSize, pageNum);
        }
        public ComboBoxPagedResult GetBinLocation(string searchTerm, int pageSize, int pageNum, int filterid)
        {
            return new BinLocationService(_unitOfWork).GetList(searchTerm, pageSize, pageNum, filterid);
        }
        public ComboBoxResult GetBinLocationById(int Id)
        {
            return new BinLocationService(_unitOfWork).GetValue(Id);
        }

        public ComboBoxPagedResult GetArea(string searchTerm, int pageSize, int pageNum)
        {
            return new AreaService(_unitOfWork).GetList(searchTerm, pageSize, pageNum);
        }
        public ComboBoxResult GetAreaById(int Id)
        {
            return new AreaService(_unitOfWork).GetValue(Id);
        }


        public ComboBoxPagedResult GetPersonRateGroup(string searchTerm, int pageSize, int pageNum)
        {
            return new PersonRateGroupService(_unitOfWork).GetList(searchTerm, pageSize, pageNum);
        }
        public ComboBoxResult GetPersonRateGroupById(int Id)
        {
            return new PersonRateGroupService(_unitOfWork).GetValue(Id);
        }



        public ComboBoxPagedResult GetBillingType(string searchTerm, int pageSize, int pageNum)
        {
            List<ComboBoxResult> list = new List<ComboBoxResult>();
            list.Add(new ComboBoxResult { id = "Regular", text = "Regular" });
            list.Add(new ComboBoxResult { id = "Juridicial", text = "Juridicial" });
            list.Add(new ComboBoxResult { id = "Hold", text = "Hold" });

            //var temp = list
            //   .Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();


            ComboBoxPagedResult Data = new ComboBoxPagedResult();
            Data.Results = list;

            return Data;
        }
        public ComboBoxResult GetBillingTypeById(string Id)
        {
            ComboBoxResult ProductJson = new ComboBoxResult();

            ProductJson.id = Id;
            ProductJson.text = Id;

            return ProductJson;
        }



        public ComboBoxPagedResult GetLedgerAccount(string searchTerm, int pageSize, int pageNum)
        {
            return new LedgerAccountService(_unitOfWork).GetList(searchTerm, pageSize, pageNum);
        }
        public ComboBoxResult GetLedgerAccountById(int Id)
        {
            return new LedgerAccountService(_unitOfWork).GetValue(Id);
        }


        public ComboBoxPagedResult GetPaymentMode(string searchTerm, int pageSize, int pageNum)
        {
            return new PaymentModeService(_unitOfWork).GetList(searchTerm, pageSize, pageNum);
        }
        public ComboBoxResult GetPaymentModeById(int Id)
        {
            return new PaymentModeService(_unitOfWork).GetValue(Id);
        }



        public ComboBoxPagedResult GetCostCenter(string searchTerm, int pageSize, int pageNum)
        {
            return new CostCenterService(_unitOfWork).GetList(searchTerm, pageSize, pageNum);
        }
        public ComboBoxResult GetCostCenterById(int Id)
        {
            return new CostCenterService(_unitOfWork).GetValue(Id);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
