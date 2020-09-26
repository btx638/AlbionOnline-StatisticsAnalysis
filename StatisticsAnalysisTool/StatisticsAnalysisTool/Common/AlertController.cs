﻿using FontAwesome.WPF;
using log4net;
using StatisticsAnalysisTool.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace StatisticsAnalysisTool.Common
{
    public class AlertController
    {
        private readonly ObservableCollection<Alert> _alerts = new ObservableCollection<Alert>();
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int _maxAlertsAtSameTime = 10;

        private void Add(ref ImageAwesome imageAwesome, ref Item item)
        {
            if (IsAlertInCollection(item.UniqueName) || !IsSpaceInAlertsCollection())
            {
                return;
            }

            _alerts.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                }
            };

            var alert = new Alert(ref imageAwesome, ref item);
            alert.StartEvent();
            _alerts.Add(alert);
        }

        private void Remove(ref Item item)
        {
            _alerts.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                }
            };

            var alert = GetAlertByUniqueName(item.UniqueName);
            if (alert != null)
            {
                alert.StopEvent();
                _alerts.Remove(alert);
            }
        }

        public bool ToggleAlert(ref ImageAwesome imageAwesome, ref Item item)
        {
            try
            {
                if (IsAlertInCollection(item.UniqueName))
                {
                    Remove(ref item);
                    return false;
                }
                else
                {
                    Add(ref imageAwesome, ref item);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error(nameof(ToggleAlert), e);
                return false;
            }
        }
        
        private bool IsAlertInCollection(string uniqueName) => _alerts.Any(alert => alert.Item.UniqueName == uniqueName);

        private Alert GetAlertByUniqueName(string uniqueName)
        {
            return _alerts.FirstOrDefault(alert => alert.Item.UniqueName == uniqueName);
        }

        private bool IsSpaceInAlertsCollection() => _alerts.Count < _maxAlertsAtSameTime;
    }
}