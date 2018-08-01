[assembly: WebActivator.PreApplicationStartMethod(typeof(Web365.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Web365.App_Start.NinjectWebCommon), "Stop")]

namespace Web365.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Web365Business.Back_End.IRepository;
    using Web365Business.Back_End.Repository;
    using Web365Business.Front_End.IRepository;
    using Web365Business.Front_End.Repository;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //Back-end
            kernel.Bind<IProductRepository>().To<ProductRepository>();
            kernel.Bind<IProductTypeRepository>().To<ProductTypeRepository>();
            kernel.Bind<IDistributorRepository>().To<DistributorRepository>();
            kernel.Bind<IManufacturerRepository>().To<ManufacturerRepository>();
            kernel.Bind<IProductStatusRepository>().To<ProductStatusRepository>();
            kernel.Bind<IProductStatusMapRepository>().To<ProductStatusMapRepository>();
            kernel.Bind<IArticleRepository>().To<ArticleRepository>();
            kernel.Bind<IArticleGroupRepository>().To<ArticleGroupRepository>();
            kernel.Bind<IArticleTypeRepository>().To<ArticleTypeRepository>();
            kernel.Bind<IArticleGroupMapRepository>().To<ArticleGroupMapRepository>();
            kernel.Bind<IPictureTypeRepository>().To<PictureTypeRepository>();
            kernel.Bind<IPictureRepository>().To<PictureRepository>();
            kernel.Bind<IUserPageRepository>().To<UserPageRepository>();
            kernel.Bind<IUserRoleRepository>().To<UserRoleRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IAdvertiesRepository>().To<AdvertiesRepository>();
            kernel.Bind<IAdvertiesPictureMapRepository>().To<AdvertiesPictureMapRepository>();
            kernel.Bind<ISupportTypeRepository>().To<SupportTypeRepository>();
            kernel.Bind<ISupportRepository>().To<SupportRepository>();
            kernel.Bind<IFileTypeRepository>().To<FileTypeRepository>();
            kernel.Bind<IFileRepository>().To<FileRepository>();
            kernel.Bind<ICustomerRepository>().To<CustomerRepository>();
            kernel.Bind<IOrderRepository>().To<OrderRepository>();
            kernel.Bind<IProductFilterRepository>().To<ProductFilterRepository>();
            kernel.Bind<IProductGroupAttributeRepository>().To<ProductGroupAttributeRepository>();
            kernel.Bind<IProductAttributeRepository>().To<ProductAttributeRepository>();
            kernel.Bind<IProductLabelRepository>().To<ProductLabelRepository>();
            kernel.Bind<ILayoutContentRepository>().To<LayoutContentRepository>();
            kernel.Bind<IContactRepository>().To<ContactRepository>();
            kernel.Bind<IReceiveInfoGroupRepository>().To<ReceiveInfoGroupRepository>();
            kernel.Bind<IReceiveInfoRepository>().To<ReceiveInfoRepository>();
            kernel.Bind<IArticleGroupTypeRepository>().To<ArticleGroupTypeRepository>();
            kernel.Bind<IArticleGroupTypeMapRepository>().To<ArticleGroupTypeMapRepository>();
            kernel.Bind<IProductTypeGroupRepository>().To<ProductTypeGroupRepository>();
            kernel.Bind<IProductTypeGroupMapRepository>().To<ProductTypeGroupMapRepository>();
            kernel.Bind<IMenuRepository>().To<MenuRepository>();
            kernel.Bind<IProductVariantRepository>().To<ProductVariantRepository>();
            kernel.Bind<ICommentRepository>().To<CommentRepository>();
            kernel.Bind<ITypeArticleAutoRepository>().To<TypeArticleAutoRepository>();
            kernel.Bind<IAutoArticleRepository>().To<AutoArticleRepository>();
            kernel.Bind<IGamePlayerRepository>().To<GamePlayerRepository>();
            kernel.Bind<IDaiLyRepository>().To<GameDaiLyRepository>();

            //Font-end
            kernel.Bind<IArticleRepositoryFE>().To<ArticleRepositoryFE>();
            kernel.Bind<IProductRepositoryFE>().To<ProductRepositoryFE>();
            kernel.Bind<ILayoutContentRepositoryFE>().To<LayoutContentRepositoryFE>();
            kernel.Bind<IAdvertiesRepositoryFE>().To<AdvertiesRepositoryFE>();
            kernel.Bind<IArticleTypeRepositoryFE>().To<ArticleTypeRepositoryFE>();
            kernel.Bind<IOtherRepositoryFE>().To<OtherRepositoryFE>();
            kernel.Bind<IOrderRepositoryFE>().To<OrderRepositoryFE>();
            kernel.Bind<IMenuRepositoryFE>().To<MenuRepositoryFE>();
            kernel.Bind<IProductTypeRepositoryFE>().To<ProductTypeRepositoryFE>();
            kernel.Bind<IProductStatusRepositoryFE>().To<ProductStatusRepositoryFE>();
            kernel.Bind<IProductFilterRepositoryFE>().To<ProductFilterRepositoryFE>();
            kernel.Bind<ICustomerRepositoryFE>().To<CustomerRepositoryFE>();
            kernel.Bind<IPictureRepositoryFE>().To<PictureRepositoryFE>();
            kernel.Bind<IFileRepositoryFE>().To<FileRepositoryFE>();
            
        }
    }
}
