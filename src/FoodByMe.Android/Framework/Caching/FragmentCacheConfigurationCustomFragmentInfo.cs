using MvvmCross.Droid.Shared.Caching;

namespace FoodByMe.Android.Framework.Caching
{
    internal class FragmentCacheConfigurationCustomFragmentInfo : FragmentCacheConfiguration<MainActivityFragmentCacheInfoFactory.SerializableCustomFragmentInfo>
    {
        private readonly MainActivityFragmentCacheInfoFactory _mainActivityFragmentCacheInfoFactory;
        public FragmentCacheConfigurationCustomFragmentInfo()
        {
            _mainActivityFragmentCacheInfoFactory = new MainActivityFragmentCacheInfoFactory();
        }

        public override MvxCachedFragmentInfoFactory MvxCachedFragmentInfoFactory => _mainActivityFragmentCacheInfoFactory;
    }
}