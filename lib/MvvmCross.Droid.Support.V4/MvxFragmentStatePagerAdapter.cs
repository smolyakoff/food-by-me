﻿// MvxFragmentStatePagerAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.Droid.Support.V4
{
    internal class MvxFragmentStatePagerAdapter
        : FragmentStatePagerAdapter
    {
        private readonly Context _context;
        public IEnumerable<FragmentInfo> Fragments { get; private set; }

        public override int Count => Fragments.Count();

        protected MvxFragmentStatePagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public MvxFragmentStatePagerAdapter(
            Context context, FragmentManager fragmentManager, IEnumerable<FragmentInfo> fragments)
            : base(fragmentManager)
        {
            _context = context;
            Fragments = fragments;

            throw new InvalidOperationException("MvxFragmentStatePagerAdapter has broken cache implementation, use MvxFragmentPagerAdapter at this moment.");
        }

        public override Fragment GetItem(int position)
        {
            var fragInfo = Fragments.ElementAt(position);

            if (fragInfo.CachedFragment == null)
            {
                fragInfo.CachedFragment = Fragment.Instantiate(_context, FragmentJavaName(fragInfo.FragmentType));

                var request = new MvxViewModelRequest (fragInfo.ViewModelType, null, null, null);
                ((IMvxView)fragInfo.CachedFragment).ViewModel = Mvx.Resolve<IMvxViewModelLoader>().LoadViewModel(request, null);
            }

            return fragInfo.CachedFragment;
        }

        protected static string FragmentJavaName(Type fragmentType)
        {
            var namespaceText = fragmentType.Namespace ?? "";
            if (namespaceText.Length > 0)
                namespaceText = namespaceText.ToLowerInvariant() + ".";
            return namespaceText + fragmentType.Name;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(Fragments.ElementAt(position).Title);
        }

        public override void RestoreState (IParcelable state, ClassLoader loader)
        {
            //Don't call restore to prevent crash on rotation
            //base.RestoreState (state, loader);
        }

        public class FragmentInfo
        {
            public FragmentInfo(string title, Type fragmentType, Type viewModelType)
            {
                Title = title;
                FragmentType = fragmentType;
                ViewModelType = viewModelType;
            }

            public string Title { get; set; }
            public Type FragmentType { get; private set; }
            public Type ViewModelType { get; private set; }
            public Fragment CachedFragment { get; set; }
        }
    }
}

