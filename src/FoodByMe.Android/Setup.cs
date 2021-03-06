using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Android.Content;
using FoodByMe.Android.Framework;
using FoodByMe.Android.Utilities;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Framework;
using FoodByMe.Core.Resources;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Shared.Presenter;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Platform;

namespace FoodByMe.Android
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
            Text.Culture = new CultureInfo("ru-RU");
            return new Core.App();
        }

		protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
		{
			typeof(global::Android.Support.Design.Widget.NavigationView).Assembly,
			typeof(global::Android.Support.Design.Widget.FloatingActionButton).Assembly,
			typeof(global::Android.Support.V7.Widget.Toolbar).Assembly,
			typeof(global::Android.Support.V4.Widget.DrawerLayout).Assembly,
			typeof(global::Android.Support.V4.View.ViewPager).Assembly,
			typeof(MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerView).Assembly
		};

        protected override void InitializeFirstChance()
        {
            Mvx.RegisterSingleton(typeof(IPictureOptimizer), () => new PictureOptimizer());
            base.InitializeFirstChance();
        }

        protected override IEnumerable<Assembly> ValueConverterAssemblies => 
            new List<Assembly>(base.ValueConverterAssemblies)
        {
            typeof (CookingTimeValueConverter).Assembly
        };

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
			return new MvxFragmentsPresenter(AndroidViewAssemblies);
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite("Language", new MvxLanguageConverter());
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            MvxAppCompatSetupHelper.FillTargetFactories(registry);
        }
    }
}