using System;
using Microsoft.Practices.Unity;
using Data.Customize;
using Infrastructure.IO;
using Models.Customize.Models;
using Services.Customize;
using Models.BasicSetup.Models;
using Services.BasicSetup;
using Models.Company.Models;
using Components.Logging;
using AutoMapper;
using Models.Customize.ViewModels;
using TimePlanValidator.ViewModels;
using Models.Company.ViewModels;
using Models.BasicSetup.ViewModels;
using Components.ExceptionHandlers;
using TimePlanValidator;
using TimePlanValidator.Models;
using Service;
using Models.Company.DatabaseViews;

namespace Customize.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<ApplicationDbContext, ApplicationDbContext>("New");

            container.RegisterType<IExceptionHandlingService, ExceptionHandlingService>(new PerRequestLifetimeManager());

            container.RegisterType<IDataContext, ApplicationDbContext>(new PerRequestLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager());



            container.RegisterType<IRepository<Godown>, Repository<Godown>>();
            container.RegisterType<IGodownService, GodownService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Site>, Repository<Site>>();
            container.RegisterType<ISiteService, SiteService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Dimension1>, Repository<Dimension1>>();
            container.RegisterType<IDimension1Service, Dimension1Service>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Product>, Repository<Product>>();
            container.RegisterType<IProductService, ProductService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<BinLocation>, Repository<BinLocation>>();
            container.RegisterType<IBinLocationService, BinLocationService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Area>, Repository<Area>>();
            container.RegisterType<IAreaService, AreaService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PersonRateGroup>, Repository<PersonRateGroup>>();
            container.RegisterType<IPersonRateGroupService, PersonRateGroupService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<PaymentMode>, Repository<PaymentMode>>();
            container.RegisterType<IRepository<PaymentModeLedgerAccount>, Repository<PaymentModeLedgerAccount>>();


            container.RegisterType<IRepository<LedgerHeader>, Repository<LedgerHeader>>();
            container.RegisterType<IRepository<LedgerLine>, Repository<LedgerLine>>();
            container.RegisterType<IRepository<Ledger>, Repository<Ledger>>();

            container.RegisterType<IRepository<Person>, Repository<Person>>();
            container.RegisterType<IRepository<ProductBuyer>, Repository<ProductBuyer>>();

            container.RegisterType<IPaymentModeService, PaymentModeService>(new PerRequestLifetimeManager());
            container.RegisterType<IPaymentModeLedgerAccountService, PaymentModeLedgerAccountService>(new PerRequestLifetimeManager());

            
            
            
            container.RegisterType<IRepository<CostCenter>, Repository<CostCenter>>();
            container.RegisterType<ICostCenterService, CostCenterService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<CostCenterStatus>, Repository<CostCenterStatus>>();
            container.RegisterType<ICostCenterStatusService, CostCenterStatusService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<LedgerAccount>, Repository<LedgerAccount>>();
            container.RegisterType<ILedgerAccountService, LedgerAccountService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<DocumentType>, Repository<DocumentType>>();
            container.RegisterType<IDocumentTypeService, DocumentTypeService>(new PerRequestLifetimeManager());

            container.RegisterType<ILogger, LogActivity>();
            container.RegisterType<IRepository<ActivityLog>, Repository<ActivityLog>>();

            container.RegisterType<IModificationCheck, ModificationCheck>();

            container.RegisterType<IRepository<Stock>, Repository<Stock>>();
            container.RegisterType<IStockService, StockService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<StockAdj>, Repository<StockAdj>>();
            container.RegisterType<IStockAdjService, StockAdjService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<StockHeader>, Repository<StockHeader>>();
            container.RegisterType<IStockHeaderService, StockHeaderService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<StockBalance>, Repository<StockBalance>>();
            container.RegisterType<IStockBalanceService, StockBalanceService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<StockProcess>, Repository<StockProcess>>();
            container.RegisterType<IStockProcessService, StockProcessService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<Process>, Repository<Process>>();
            container.RegisterType<IProcessService, ProcessService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<StockProcessBalance>, Repository<StockProcessBalance>>();
            container.RegisterType<IStockProcessBalanceService, StockProcessBalanceService>(new PerRequestLifetimeManager());




            container.RegisterType<IChargesCalculationService, ChargesCalculationService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<CalculationProduct>, Repository<CalculationProduct>>();
            container.RegisterType<ICalculationProductService, CalculationProductService>(new PerRequestLifetimeManager());

            container.RegisterType<IRepository<CalculationFooter>, Repository<CalculationFooter>>();
            container.RegisterType<ICalculationFooterService, CalculationFooterService>(new PerRequestLifetimeManager());

            container.RegisterType<INotificationService, NotificationService>(new PerRequestLifetimeManager());

            container.RegisterType<ICustomizeComboHelpListService, CustomizeComboHelpListService>(new PerRequestLifetimeManager());

            container.RegisterType<IExceptionHandler, ExceptionHandler>();

            container.RegisterType<IDocumentValidation, DocumentValidation>();
            container.RegisterType<IRepository<DocumentTypeTimeExtension>, Repository<DocumentTypeTimeExtension>>();
            container.RegisterType<IRepository<DocumentTypeTimePlan>, Repository<DocumentTypeTimePlan>>();

            container.RegisterType<IRepository<Product>, Repository<Product>>();
            container.RegisterType<IRepository<Unit>, Repository<Unit>>();

            container.RegisterType<IRepository<Product>, Repository<Product>>();

            Mapper.Initialize(m => {
                m.CreateMap<PaymentMode, PaymentModeViewModel>();
                m.CreateMap<Person, DocumentUniqueId>();
            m.CreateMap<LedgerHeader, DocumentUniqueId>();

            m.CreateMap<PaymentModeViewModel, DocumentUniqueId>();
            m.CreateMap<HeaderChargeViewModel, HeaderChargeViewModel>();
            m.CreateMap<LineChargeViewModel, LineChargeViewModel>();
            m.CreateMap<CalculationFooterViewModel, HeaderChargeViewModel>();
            m.CreateMap<CalculationProductViewModel, LineChargeViewModel>();
            m.CreateMap<PaymentMode, PaymentModeViewModel>();
            m.CreateMap<PaymentModeViewModel, PaymentMode>();
            m.CreateMap<PaymentModeLedgerAccount, PaymentModeViewModel>();
            m.CreateMap<PaymentModeViewModel, PaymentModeLedgerAccount>();
            m.CreateMap<DocumentTypeAttribute, DocumentTypeAttributeViewModel>();
            });
        }
    }
}
