﻿using Caliburn.Micro;
using StacjaBenzynowaMVVM.Helpers;
using StacjaBenzynowaMVVM.Helpers.Classes;
using StacjaBenzynowaMVVM.Models;
using StacjaBenzynowaMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StacjaBenzynowaMVVM
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        #region Constructor
        public Bootstrapper()
        {
            Initialize();
            //ConventionManager.AddElementConvention<PasswordBox>();
        }
        #endregion

        #region Overrides
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
             DisplayRootViewFor<ShellViewModel>();
        }

        protected override void Configure()
        {
            _container.Instance(_container);
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();
            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");


            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(viewModelType, viewModelType.ToString(), viewModelType));
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }
        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        #endregion

    }
}
